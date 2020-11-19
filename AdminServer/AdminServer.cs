using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using Service;
using TCPComm;
using TCPComm.Protocol;

namespace AdminServer
{
    public class AdminServer
    {
        static TcpListener server;
        private static UserService _userService;
    
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting server");
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
            server = new TcpListener(ipEndPoint);
            server.Start(100);

            while (true)
            {
                var client = await server.AcceptTcpClientAsync();
                Task.Run(() => WaitForClientConnection(client));
            }
        }
        
        static async Task WaitForClientConnection(TcpClient client)
        {
            var stream = client.GetStream();
            var communication = new NetworkCommunication(stream);
            await Task.Run(async () => await ReadAsync(communication));
        }

        static async Task WriteAsync(NetworkCommunication communication, ResponseDTO response)
        {
            await communication.WriteAsync(ParseWrite(response));
        }
    
        static byte[] ParseWrite(ResponseDTO response)
        {
            byte[] result = new byte[0];
            result = Utils.AddCommandResult(result, "RES");
            result = Utils.AddCommandResult(result, response.Code.ToString());

            switch (response.Code)
            {
                case ProtocolConstants.ResponseCommands.OK:
                    result = Utils.AddLengthResult(result, ProtocolConstants.MessageSize);
                    result = Utils.MergeArrays(result,Utils.FillToSize(response.Message,ProtocolConstants.MessageSize));
                    break;
                case ProtocolConstants.ResponseCommands.ERROR:
                    result = Utils.AddLengthResult(result, ProtocolConstants.MessageSize);
                    result = Utils.MergeArrays(result,Utils.FillToSize(response.Message,ProtocolConstants.MessageSize));
                    break;
            }

            return result;
        }

        static async Task ReadAsync(NetworkCommunication communication)
        {
            while (true)
            {
                var command = Utils.ByteToString(await communication.ReadAsync(ProtocolConstants.CommandSize));
                var codeString = Utils.ByteToString(await communication.ReadAsync(ProtocolConstants.RequestSize));
                var code = 0;
                var dataLength = Utils.ByteToInt(await communication.ReadAsync(ProtocolConstants.IntegerSize));
                
                switch (code)
                {
                    case (int)ProtocolConstants.RequestCommands.USER_CREATE:
                        var nameAdd = Utils.ByteToString(await communication.ReadAsync(User.NameSize));
                        var emailAdd = Utils.ByteToString(await communication.ReadAsync(User.EmailSize));
                        var userToAdd = new User()
                        {
                            Name = nameAdd,
                            Email = emailAdd
                        };
            
                        try
                        {
                            _userService.AddUser(userToAdd);
                            var response = new ResponseDTO()
                            {
                                Message = "Added Sucessfully",
                                Code = ProtocolConstants.ResponseCommands.OK
                            };
                            await WriteAsync(communication, response);
                        }
                        catch (Exception e)
                        {
                            //TODO 
                        }

                        break;
                    case (int)ProtocolConstants.RequestCommands.USER_DELETE:
                        var emailDelete = Utils.ByteToString(await communication.ReadAsync(User.EmailSize));
                        var userToDelete = new User()
                        {
                            Email = emailDelete
                        };

                        try
                        {
                            _userService.DeleteUser(userToDelete);
                            var response = new ResponseDTO()
                            {
                                Message = "Deleted Successfully",
                                Code = ProtocolConstants.ResponseCommands.OK
                            };
                            await WriteAsync(communication, response);
                        }
                        catch (Exception e)
                        {
                            //TODO 
                        }
 
                        break;
                    case (int)ProtocolConstants.RequestCommands.USER_MODIFIY:
                        var nameNew = Utils.ByteToString(await communication.ReadAsync(User.NameSize));
                        var emailNew = Utils.ByteToString(await communication.ReadAsync(User.EmailSize));
                        var userToModify = new User()
                        {
                            Name = nameNew,
                            Email = emailNew
                        };
            
                        try
                        {
                            _userService.ModifyUser(userToModify);
                            var response = new ResponseDTO()
                            {
                                Message = "Modified Successfully",
                                Code = ProtocolConstants.ResponseCommands.OK
                            };
                            await WriteAsync(communication, response);
                        }
                        catch (Exception e)
                        {
                            //TODO 
                        }

                        break;
                }
            }
        }
    }
}