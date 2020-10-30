using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TCPComm.Protocol;

namespace Controller.FileClient
{
    public class FileClient
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting client...");
            var clientIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            TcpClient client = new TcpClient(clientIpEndPoint);
            Console.WriteLine("Attempting connection to server...");
            await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 6000);
            NetworkStream stream = client.GetStream();
            var communication = new NetworkCommunication(stream);
            while (true)
            {
                var writeTask = Task.Run(async () => await WriteAsync(communication));
                var readTask = Task.Run(async () => await ReadAsync(communication));
            }
        }

        static async Task WriteAsync(NetworkCommunication communication)
        {
            var data = parseWrite(Console.ReadLine());
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            await communication.WriteAsync(Encoding.UTF8.GetBytes("REQ"));
            await communication.WriteAsync(dataLength);
            await communication.WriteAsync(data);
        }

        static byte[] parseWrite(string commandConsole)
        {
            var splitedCommand = commandConsole.Split(',');
            var result = new byte[0];
            
            switch (Int32.Parse(splitedCommand[0]))
            {
                case (int)ProtocolConstants.REQUEST_COMMANDS.LOGIN:
                    result = Encoding.UTF8.GetBytes(splitedCommand[1]);
                    break;
                case (int)ProtocolConstants.REQUEST_COMMANDS.USER_CREATE:
                    result = (byte[])Encoding.UTF8.GetBytes(splitedCommand[1]).Concat(Encoding.UTF8.GetBytes(splitedCommand[2]));
                    break;
                case (int)ProtocolConstants.REQUEST_COMMANDS.USER_DELETE:
                    result = Encoding.UTF8.GetBytes(splitedCommand[1]);
                    break;
                case (int)ProtocolConstants.REQUEST_COMMANDS.PHOTO_GET:
                    result = Encoding.UTF8.GetBytes(splitedCommand[1]);
                    break;
                case (int)ProtocolConstants.REQUEST_COMMANDS.PHOTO_LOAD:
                    result = getFileFromFS(splitedCommand[1]);
                    //TODO
                    //OBTENER LOS ARCHIVOS DE LA RUTA
                    break;
                case (int)ProtocolConstants.REQUEST_COMMANDS.COMMENT_PHOTO:
                    result = (byte[])Encoding.UTF8.GetBytes(splitedCommand[1]).Concat(Encoding.UTF8.GetBytes(splitedCommand[2]));
                    break;
                case (int)ProtocolConstants.REQUEST_COMMANDS.PHOTO_COMMENTS:
                    result = Encoding.UTF8.GetBytes(splitedCommand[1]);
                    break;
                default:
                    result = new byte[0];
                    break;
            }

            return result;
        }

        private static byte[] getFileFromFS(string v)
        {
            throw new NotImplementedException();
        }

        static async Task ReadAsync(NetworkCommunication communication)
        {
            _ = await communication.ReadAsync(ProtocolConstants.COMMAND_SIZE);
            var responseCode = BitConverter.ToInt32(await communication.ReadAsync(ProtocolConstants.REQUEST_SIZE));
            var msg = "";
            var dataLength = ByteToInt(await communication.ReadAsync(ProtocolConstants.COMMAND_SIZE));

            switch (responseCode)
            {
                case 0:
                    msg = ByteToString(await communication.ReadAsync(ProtocolConstants.MESSAGE_SIZE));
                    break;
                case 1:
                    msg = ByteToString(await communication.ReadAsync(ProtocolConstants.MESSAGE_SIZE));
                    break;
                case 2:
                    dataLength = ByteToInt(await communication.ReadAsync(ProtocolConstants.COMMAND_SIZE));
                    var users = await GetClientsAsync(communication, dataLength);
                    msg = UsersToString(users);
                    break;
                case 3:
                    dataLength = ByteToInt(await communication.ReadAsync(ProtocolConstants.COMMAND_SIZE));
                    var photos = await GetPhotosAsync(communication, dataLength);
                    msg = PhotosToString(photos);
                    break;
                case 4:
                    //dataLength = ByteToInt(await communication.ReadAsync(ProtocolConstants.COMMAND_SIZE));
                    //var users = await ParseUserListAsync(communication, dataLength);
                    //msg = UsersToString(users);
                    break;
            }
            //int dataSize = BitConverter.ToInt32(dataLength, 0);
            //byte[] data = await communication.ReadAsync(dataSize);
            Console.WriteLine(msg);
        }

        private static string UsersToString(List<Client> clients)
        {
            throw new NotImplementedException();
        }

        private static string PhotosToString(List<Photo> photos)
        {
            throw new NotImplementedException();
        }


        private static async Task<List<Client>> GetClientsAsync(NetworkCommunication communication, int dataLength)
        {
            var users = new List<Client>();

            while (dataLength!=0)
            {
                var name = ByteToString(await communication.ReadAsync(ProtocolConstants.USER_NAME));
                var email = ByteToString(await communication.ReadAsync(ProtocolConstants.USER_EMAIL));
                var lastConnection = ByteToDateTime(await communication.ReadAsync(ProtocolConstants.LAST_CONNECTION));
                var user = new Client()
                {
                    Name = name,
                    Email = email,
                    LastConnection = lastConnection,
                };

                users.Add(user);

                dataLength -= ProtocolConstants.USER_NAME + ProtocolConstants.USER_EMAIL + ProtocolConstants.LAST_CONNECTION;
            }

            return users;
        }

        //private static async Task<List<Photo>> GetPhotosAsync(NetworkCommunication communication, int dataLength)
        //{
        //    var users = new List<Client>();

        //    while (dataLength != 0)
        //    {
        //        var id = ByteToString(await communication.ReadAsync(ProtocolConstants.USER_NAME));
        //        var email = ByteToString(await communication.ReadAsync(ProtocolConstants.USER_EMAIL));
        //        var lastConnection = ByteToDateTime(await communication.ReadAsync(ProtocolConstants.LAST_CONNECTION));
        //        var user = new Photo()
        //        {
        //            Name = name,
        //            Email = email,
        //            LastConnection = lastConnection,
        //        };

        //        users.Add(user);

        //        dataLength -= ProtocolConstants.USER_NAME + ProtocolConstants.USER_EMAIL + ProtocolConstants.LAST_CONNECTION;
        //    }

        //    return users;
        //}


        static string ByteToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        static DateTime ByteToDateTime(byte[] data)
        {
            return Convert.ToDateTime(data);
        }

        static int ByteToInt(byte[] data)
        {
            return BitConverter.ToInt32(data);
        }
    }
}
