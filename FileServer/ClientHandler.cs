using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using TCPComm;
using TCPComm.Protocol;

namespace FileServer
{
    public static class ClientHandler
    {
        private static readonly string PhotosPath = AppDomain.CurrentDomain.BaseDirectory + "Photos";

        public static async Task ValidateLogin(FileServer.Server server, CommunicationClient client)
        {
            var existUser = false;
            do
            {
                var email = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserEmailLength));
                var password = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserPasswordLength));
                var user = new User
                {
                    Email = email,
                    Password = password
                };
                
                existUser = server.Service.AutenticateUser(user);
                
                if (!existUser)
                {
                    ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                    client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Invalid User", ProtocolConstants.ResponseMessageLength));
                }
                else
                {
                    client.User = user;

                    ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                    client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Login Successfully", ProtocolConstants.ResponseMessageLength));
                }
            } while (!existUser);
        }
        
        public static async Task HandleCreateUser(FileServer.Server server, CommunicationClient client)
        {
            var name = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserNameLength));
            var email = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserEmailLength));
            var password = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserPasswordLength));

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Input Error", ProtocolConstants.ResponseMessageLength));
            }

            var user = new User
            {
                Email = email,
                Name = name,
                Password = password,
            };
            
            server.Service.AddUser(user);
            client.User = user;

            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Ok ,client.StreamCommunication);
            client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Added Sucessfully", ProtocolConstants.ResponseMessageLength));
        }

        public static async Task HandleUploadPhoto(FileServer.Server server, CommunicationClient client)
        {
            var name = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(Photo.PhotoNameLength));
            var extension = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(Photo.PhotoExtensionLength));
            var fileSize = ConversionHandler.ConvertBytesToLong( await client.StreamCommunication.ReadAsync(ProtocolConstants.LongTypeLength));
                        
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(extension))
            {
                ProtocolHelpers.SendMessageCommand(ProtocolConstants.ResponseCommands.Ok, client, "Input Error");
            }

            var photo = new Photo()
            {
                Name = name,
                Extension = extension,
                FileSize = fileSize,
                User = client.User
            };

            server.Service.UploadPhoto(photo);
            var fileName = $"{PhotosPath}\\Image_{photo.Id}{extension}";
            
            await FileHandler.ReceiveFileWithStreams(fileSize, fileName, client.StreamCommunication);

            ProtocolHelpers.SendMessageCommand(ProtocolConstants.ResponseCommands.Ok, client, "Added succesfully");
        }

        public static async Task HandleCommentPhoto(FileServer.Server server, CommunicationClient client)
        {
            var photoIdParsed = ConversionHandler.ConvertBytesToInt( await client.StreamCommunication.ReadAsync(ProtocolConstants.IntegerTypeLength));
            var message = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(Comment.CommentLength));
            
            if (string.IsNullOrWhiteSpace(message))
            {
                ProtocolHelpers.SendMessageCommand(ProtocolConstants.ResponseCommands.Ok, client, "Input Error");
            }

            var comment = new Comment
            {
                Photo = new Photo
                {
                    Id = photoIdParsed
                },
                Message = message,
                Commentor = client.User
            };

            server.Service.CommentPhoto(comment);

            ProtocolHelpers.SendMessageCommand(ProtocolConstants.ResponseCommands.Ok,client, "Added Sucessfully");
        }

        public static void HandleViewUsers(FileServer.Server server, CommunicationClient client)
        {
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.ListUsers,
                client.StreamCommunication);

            var clients = server.Service.GetAllClients();
            var length = clients.Count() * (User.UserEmailLength + User.UserNameLength + ProtocolConstants.DateTimeTypeLength);

            var data = ConversionHandler.ConvertIntToBytes(length);
            client.StreamCommunication.Write(data);
            clients.ForEach((elem) =>
            {
                ProtocolHelpers.SendUserData(client.StreamCommunication,elem);
            });
        }

        public static void HandleViewPhotos(FileServer.Server server, CommunicationClient client)
        {
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.ListPhotos,
            client.StreamCommunication);

            var photos = server.Service.GetPhotos();
            var length = photos.Count() * (User.UserEmailLength + ProtocolConstants.LongTypeLength + Photo.PhotoNameLength + Photo.PhotoExtensionLength + ProtocolConstants.LongTypeLength);

            var data = ConversionHandler.ConvertIntToBytes(length);
            client.StreamCommunication.Write(data);
            photos.ForEach((elem) =>
            {
                ProtocolHelpers.SendPhotoData(client.StreamCommunication, elem);
            });
        }

        public static async Task HandleViewCommentsPhoto(FileServer.Server server, CommunicationClient client)
        {
            var photoIdParsed = ConversionHandler.ConvertBytesToInt( await client.StreamCommunication.ReadAsync(ProtocolConstants.IntegerTypeLength));
    
            var photo = new Photo
            {
                Id = photoIdParsed
            };

            server.Service.GetCommentsFromPhoto(photo).ForEach((elem) =>
            {
                ProtocolHelpers.SendCommentData(client.StreamCommunication,elem);
            });
        }
    }
}