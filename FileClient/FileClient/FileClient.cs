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
            var data = (byte[])Encoding.UTF8.GetBytes("REQ").Concat(ParseWrite(Console.ReadLine()));
            await communication.WriteAsync(data);
        }

        static byte[] ParseWrite(string commandConsole)
        {
            var splitedCommand = commandConsole.Split(',');
            byte[] result = new byte[0];
            result = AddCommandResult(result, splitedCommand[0]);
            switch (int.Parse(splitedCommand[0]))
            {
                case (int)ProtocolConstants.RequestCommands.LOGIN:
                    result = AddLengthResult(result, User.EmailSize);
                    result = MergeArrays(result,FillToSize(splitedCommand[1], User.EmailSize));
                    break;
                case (int)ProtocolConstants.RequestCommands.USER_CREATE:
                    result = AddLengthResult(result, User.EmailSize+User.NameSize);
                    result = MergeArrays(result,FillToSize(splitedCommand[1], User.EmailSize));
                    var userData = MergeArrays(FillToSize(splitedCommand[1], User.NameSize),
                        FillToSize(splitedCommand[2], User.NameSize));
                    result = MergeArrays(result,userData);
                    break;
                case (int)ProtocolConstants.RequestCommands.USER_DELETE:
                    result = AddLengthResult(result, User.EmailSize);
                    result = MergeArrays(result,FillToSize(splitedCommand[1], User.EmailSize));
                    break;
                case (int)ProtocolConstants.RequestCommands.PHOTO_GET:
                    result = AddLengthResult(result, User.EmailSize);
                    result = MergeArrays(result,FillToSize(splitedCommand[1], User.EmailSize));
                    break;
                case (int)ProtocolConstants.RequestCommands.PHOTO_LOAD:
                    result = GetFileFromFs(splitedCommand[1]);
                    //TODO
                    //OBTENER LOS ARCHIVOS DE LA RUTA
                    break;
                case (int)ProtocolConstants.RequestCommands.COMMENT_PHOTO:
                    result = AddLengthResult(result, User.EmailSize+User.NameSize);
                    result = MergeArrays(result,FillToSize(splitedCommand[1], User.EmailSize));
                    var commentData = MergeArrays(FillToSize(splitedCommand[1], User.EmailSize),
                        FillToSize(splitedCommand[2], User.NameSize));
                    result = MergeArrays(result,commentData);
                    break;
                case (int)ProtocolConstants.RequestCommands.PHOTO_COMMENTS:
                    result = Encoding.UTF8.GetBytes(splitedCommand[1]);
                    break;
                default:
                    result = new byte[0];
                    break;
            }

            return result;
        }

        private static byte[] AddCommandResult(byte[] result, string cmd)
        {
            return MergeArrays(result,FillToSize(cmd, ProtocolConstants.CommandSize));
        }

        private static byte[] AddLengthResult(byte[] result, int size)
        {
            return MergeArrays(result,FillToSize(size.ToString(), ProtocolConstants.CommandSize));
        }

        static async Task ReadAsync(NetworkCommunication communication)
        {
            await communication.ReadAsync(ProtocolConstants.CommandSize);
            var responseCode = BitConverter.ToInt32(await communication.ReadAsync(ProtocolConstants.RequestSize));
            var msg = "";
            var dataLength = ByteToInt(await communication.ReadAsync(ProtocolConstants.CommandSize));

            switch (responseCode)
            {
                case (int)ProtocolConstants.ResponseCommands.OK:
                    msg = ByteToString(await communication.ReadAsync(ProtocolConstants.MessageSize));
                    break;
                case (int)ProtocolConstants.ResponseCommands.ERROR:
                    msg = ByteToString(await communication.ReadAsync(ProtocolConstants.MessageSize));
                    break;
                case (int)ProtocolConstants.ResponseCommands.LIST_USERS:
                    var users = await GetClientsAsync(communication, dataLength);
                    msg = users.ToString();
                    break;
                case (int)ProtocolConstants.ResponseCommands.LIST_PHOTOS:
                    var photos = await GetPhotosAsync(communication, dataLength);
                    msg = photos.ToString();
                    break;
                case (int)ProtocolConstants.ResponseCommands.LIST_COMMENTS:
                    var comments = await GetPhotosAsync(communication, dataLength);
                    msg = comments.ToString();
                    break;
            }

            Console.WriteLine(msg);
        }

        private static string UsersToString(IEnumerable<User> clients)
        {
            return clients.Aggregate("",(acc, x) => acc + x.Email+','+x.Name+','+x.LastConnection+'\n');
        }

        private static string PhotosToString(IEnumerable<Photo> photos)
        {
            throw new NotImplementedException();
        }

        private static async Task<IEnumerable<User>> GetClientsAsync(NetworkCommunication communication, int dataLength)
        {
            var users = new List<User>();

            while (dataLength!=0)
            {
                var name = ByteToString(await communication.ReadAsync(User.NameSize));
                var email = ByteToString(await communication.ReadAsync(User.EmailSize));
                var lastConnection = ByteToDateTime(await communication.ReadAsync(User.LastConnectionSize));
                var user = new User()
                {
                    Name = name,
                    Email = email,
                    LastConnection = lastConnection,
                };

                users.Add(user);

                dataLength -= User.NameSize + User.EmailSize + User.LastConnectionSize;
            }

            return users;
        }

        private static async Task<IEnumerable<Photo>> GetPhotosAsync(NetworkCommunication communication, int dataLength)
        {
            var photos = new List<Photo>();
        
            while (dataLength != 0)
            {
                var id = ByteToString(await communication.ReadAsync(Photo.IdSize));
                var name = ByteToString(await communication.ReadAsync(Photo.NameSize));
                var lengthFile = ByteToInt(await communication.ReadAsync(Photo.LengthSize));
                var email = ByteToString(await communication.ReadAsync(User.EmailSize));
                //TODO
                //Como hacer fotos???
                var user = new Photo()
                {
                    Id = id,
                    Name = name,
                    LengthFile = lengthFile,
                    User = new User()
                    {
                        Email = email
                    }
                };
        
                photos.Add(user);
        
                dataLength -= Photo.IdSize + Photo.NameSize + Photo.LengthSize + User.EmailSize;
            }
        
            return photos;
        }

        private static byte[] FillToSize(string v, int size)
        {
            var data = Encoding.UTF8.GetBytes(v);
            
            if (data.Length < size)
            {
                var fill = new byte[size - data.Length];
                for (int i = 0; i < fill.Length; i++)
                {
                    fill[i] = 0x0;
                }

                data = (byte[])fill.Concat(data);
            }
            
            return data;
        }
        
        private static byte[] GetFileFromFs(string v)
        {
            throw new NotImplementedException();
        }

        static byte[] MergeArrays(byte[] arr1,byte[]arr2)
        {
            return (byte[])arr1.Concat(arr2);
        }

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
