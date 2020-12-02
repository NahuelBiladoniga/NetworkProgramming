using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using TCPComm;
using TCPComm.Protocol;

namespace Server
{
    public static class ClientHandler
    {
        private static readonly string PhotosPath = AppDomain.CurrentDomain.BaseDirectory + "Photos";

        public static async Task ValidateLogin(Server server, CommunicationClient client)
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
                    client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Invalid User"));
                }
                else
                {
                    ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                    client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Login Successfully"));
                }
            } while (!existUser);
        }
        
        public static async Task HandleCreateUser(Server server, CommunicationClient client)
        {
            var name = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserNameLength));
            var email = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserEmailLength));

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Input Error"));
            }

            var user = new User
            {
                Email = email,
                Name = name
            };
            
            server.Service.AddUser(user);
            
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Ok ,client.StreamCommunication);
            client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Added Sucessfully"));
        }

        public static async Task HandleUploadPhoto(Server server, CommunicationClient client)
        {
            var name = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(Photo.PhotoNameLength));
            var extension = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(Photo.PhotoExtensionLength));
            var fileSize = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(ProtocolConstants.LongTypeLength));
            
            var parsedFileSize = (long)0;
            
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(extension) || long.TryParse(fileSize, out parsedFileSize))
            {
                ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Input Error"));
            }
            
            var photo = new Photo()
            {
                Name = name,
                Extension = extension,
                FileSize = parsedFileSize
            };
            photo.UpdateId();
            server.Service.UploadPhoto(photo);
            var fileName = $"{PhotosPath}\\{name}_{photo.Id}.{extension}";
            
            await client.FileCommsHandler.ReceiveFileWithStreams(parsedFileSize,fileName);
            
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Ok ,client.StreamCommunication);
            client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Added Sucessfully"));
        }

        public static async Task HandleCommentPhoto(Server server, CommunicationClient client)
        {
            var photoIdParsed = ConversionHandler.ConvertBytesToInt( await client.StreamCommunication.ReadAsync(ProtocolConstants.IntegerTypeLength));
            var message = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(Comment.CommentLength));
            
            if (string.IsNullOrWhiteSpace(message))
            {
                ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Input Error"));
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
            
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Ok ,client.StreamCommunication);
            client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Added Sucessfully"));
        }

        public static void HandleViewUsers(Server server, CommunicationClient client)
        {
            ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.ListUsers,
                client.StreamCommunication);
            
            server.Service.GetAllClients().ForEach((elem) =>
            {
                ProtocolHelpers.SendUserData(client.StreamCommunication,elem);
            });
        }

        public static async Task HandleViewPhotos(Server server, CommunicationClient client)
        {
            var email = ConversionHandler.ConvertBytesToString( await client.StreamCommunication.ReadAsync(User.UserEmailLength));

            if (string.IsNullOrWhiteSpace(email))
            {
                ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Input Error"));
            }

            var user = new User
            {
                Email = email
            };

            server.Service.GetPhotosFromUser(user).ForEach((elem) =>
            {
                ProtocolHelpers.SendPhotoData(client.StreamCommunication,elem);
            });
        }

        public static async Task HandleViewCommentsPhoto(Server server, CommunicationClient client)
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