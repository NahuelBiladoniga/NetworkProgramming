using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using RepositoryClient.Dto;
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
                var user = new UserDto()
                {
                    Email = email,
                    Password = password
                };
                
                existUser = await server.Service.AutenticateUserAsync(user);
                
                if (!existUser)
                {
                    ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                    client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Invalid User", ProtocolConstants.ResponseMessageLength));
                }
                else
                {
                    client.User = new User()
                    {
                        Email = email,
                        Password = password
                    };

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
            
            var user = new UserDto()
            {
                Email = email,
                Name = name,
                Password = password,
            };
            
            await server.Service.AddUserAsync(user);
            client.User = new User()
            {
                Email = email,
                Name = name,
                Password = password,
            };

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

            var photo = new PhotoDto()
            {
                Name = name,
                Extension = extension,
                FileSize = fileSize,
            };

            await server.Service.UploadPhotoAsync(photo);
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

            var comment = new CommentDto()
            {
                PhotoId = photoIdParsed,
                Message = message,
                UserEmail = client.User.Email,
            };

            await server.Service.AddCommentAsync(comment);

            ProtocolHelpers.SendMessageCommand(ProtocolConstants.ResponseCommands.Ok,client, "Added Sucessfully");
        }

        public static async Task HandleViewUsers(FileServer.Server server, CommunicationClient client)
        {
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.ListUsers,
                client.StreamCommunication);

            var clients = (await server.Service.GetUsersAsync()).ToList();
            var length = clients.Count() * (User.UserEmailLength + User.UserNameLength + ProtocolConstants.DateTimeTypeLength);

            var data = ConversionHandler.ConvertIntToBytes(length);
            client.StreamCommunication.Write(data);
            clients.ForEach((elem) =>
            {
                var user = new User()
                {
                    Name = elem.Name,
                    Email = elem.Email,
                    LastConnection = elem.LastConnection
                };
                ProtocolHelpers.SendUserData(client.StreamCommunication,user);
            });
        }

        public static async Task HandleViewPhotos(FileServer.Server server, CommunicationClient client)
        {
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.ListPhotos,
            client.StreamCommunication);

            var photos = await server.Service.GetPhotosAsync();
            var length = photos.Count() * (User.UserEmailLength + ProtocolConstants.LongTypeLength + Photo.PhotoNameLength + Photo.PhotoExtensionLength + ProtocolConstants.LongTypeLength);

            var data = ConversionHandler.ConvertIntToBytes(length);
            client.StreamCommunication.Write(data);
            photos.ForEach((elem) =>
            {
                var photo = new Photo()
                {
                    Id = elem.Id,
                    Name = elem.Name,
                    Extension = elem.Extension,
                    FileSize = elem.FileSize,
                    User = new User()
                    {
                        Email = elem.UserEmail
                    }
                };
                ProtocolHelpers.SendPhotoData(client.StreamCommunication, photo);
            });
        }

        public static async Task HandleViewCommentsPhoto(FileServer.Server server, CommunicationClient client)
        {
            var photoIdParsed = ConversionHandler.ConvertBytesToInt( await client.StreamCommunication.ReadAsync(ProtocolConstants.IntegerTypeLength));
    
            var photo = new PhotoDto()
            {
                Id = photoIdParsed
            };

            (await server.Service.GetCommentsAsync(photo)).ToList().ForEach((elem) =>
            {
                var comment = new Comment()
                {
                    Message = elem.Message,
                    CreationDate = elem.CreationDate,
                    Commentator = new User()
                    {
                        Email = elem.UserEmail,
                        Name = elem.UserName
                    }
                };
                ProtocolHelpers.SendCommentData(client.StreamCommunication, comment);
            });
        }
    }
}