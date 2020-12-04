using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using TCPComm.Dto;
using TCPComm.Protocol;

namespace FileClient
{
    public static class ServerHandler
    {
        public static async Task<MessageResponse> HandleLogin(Client client, User user)
        {
            ProtocolHelpers.SendRequestCommand(ProtocolConstants.RequestCommands.LOGIN, client.StreamCommunication);
            
            var email = ConversionHandler.ConvertStringToBytes( user.Email, User.UserEmailLength);
            var password = ConversionHandler.ConvertStringToBytes( user.Password, User.UserPasswordLength);
                
            client.StreamCommunication.Write(email);
            client.StreamCommunication.Write(password);
            
            return await ProtocolHelpers.RecieveMessageCommand(client.StreamCommunication);
        }
        
        public static async Task<MessageResponse> HandleRegister(Client client, User user)
        {
            ProtocolHelpers.SendRequestCommand(ProtocolConstants.RequestCommands.CREATE_USER, client.StreamCommunication);
            
            var name = ConversionHandler.ConvertStringToBytes( user.Name, User.UserNameLength);
            var email = ConversionHandler.ConvertStringToBytes( user.Email, User.UserEmailLength);
            var password = ConversionHandler.ConvertStringToBytes( user.Password, User.UserPasswordLength);

            client.StreamCommunication.Write(email);
            client.StreamCommunication.Write(name);
            client.StreamCommunication.Write(password);
            var response = await ProtocolHelpers.RecieveMessageCommand(client.StreamCommunication);

            return response;
        }
        
        public static async Task<MessageResponse> HandleCommentCreation(Client client, Comment comment)
        {
            var photoIdData = ConversionHandler.ConvertLongToBytes( comment.Photo.Id);
            var commentData = ConversionHandler.ConvertStringToBytes( comment.Message, Comment.CommentLength);
         
            ProtocolHelpers.SendRequestCommand(ProtocolConstants.RequestCommands.COMMENT_PHOTO, client.StreamCommunication);

            client.StreamCommunication.Write(photoIdData);
            client.StreamCommunication.Write(commentData);

            return await ProtocolHelpers.RecieveMessageCommand(client.StreamCommunication);
        }
        
        public static async Task<MessageResponse> HandleImageUpload(Client client, string filePath)
        {
            if (!FileHandler.FileExists(filePath))
                return new MessageResponse()
                {
                    responseCommands = ProtocolConstants.ResponseCommands.Error,
                    Message = "Invalid File",
                };
            
            var nameData = ConversionHandler.ConvertStringToBytes(FileHandler.FileName(filePath), Photo.PhotoNameLength);
            var extensionData = ConversionHandler.ConvertStringToBytes(FileHandler.FileExtension(filePath), Photo.PhotoExtensionLength);
            var fileSize = FileHandler.GetFileSize(filePath);
            var fileSizeData = ConversionHandler.ConvertLongToBytes(fileSize);

            ProtocolHelpers.SendRequestCommand(ProtocolConstants.RequestCommands.UPLOAD_PHOTO, client.StreamCommunication);

            client.StreamCommunication.Write(nameData);
            client.StreamCommunication.Write(extensionData);
            client.StreamCommunication.Write(fileSizeData);
            
            FileHandler.SendFileWithStream(fileSize, filePath, client.StreamCommunication);
                
            return await ProtocolHelpers.RecieveMessageCommand(client.StreamCommunication);
        }
        
        public static async Task<List<User>> HandleViewUsers(Client client)
        {
            ProtocolHelpers.SendRequestCommand(ProtocolConstants.RequestCommands.VIEW_USERS, client.StreamCommunication);

            ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
            ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));

            var data = await client.StreamCommunication.ReadAsync(ProtocolConstants.IntegerTypeLength);
            var dataLength = ConversionHandler.ConvertBytesToInt(data);

            var result = new List<User>();
            while (dataLength != 0)
            {
                var name = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(User.UserNameLength));
                var email = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(User.UserEmailLength));
                var lastConnectionDate = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(ProtocolConstants.DateTimeTypeLength));

                dataLength -= User.UserNameLength + User.UserEmailLength + ProtocolConstants.DateTimeTypeLength;
                result.Add(new User
                    {
                        Name = name,
                        Email = email,
                        //LastConnection = DateTime.Parse(lastConnectionDate)
                    });
            }
            
            return result;
        }

        public static async Task<List<Comment>> HandleViewComments(Client client, Photo photo)
        {
            ProtocolHelpers.SendRequestCommand(ProtocolConstants.RequestCommands.VIEW_USERS, client.StreamCommunication);
            client.StreamCommunication.Write(ConversionHandler.ConvertLongToBytes(photo.Id));

            ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
            ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));

            var data = await client.StreamCommunication.ReadAsync(ProtocolConstants.LongTypeLength);
            var dataLength = ConversionHandler.ConvertBytesToLong(data);

            var result = new List<Comment>();
            while (dataLength != 0)
            {
                var name = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(User.UserNameLength));
                var email = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(User.UserEmailLength));
                var message = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(Comment.CommentLength));

                dataLength -= User.UserNameLength + User.UserEmailLength + Comment.CommentLength;
                result.Add(new Comment()
                {
                    Photo = new Photo
                    {
                        User = new User
                        {
                            Email = email,
                            Name = name
                        }
                    },
                    Message = message,
                });
            }
            
            return result;
        }

        public static async Task<List<Photo>> HandleViewPhotos(Client client)
        {
            ProtocolHelpers.SendRequestCommand(ProtocolConstants.RequestCommands.VIEW_PHOTOS, client.StreamCommunication);

            var response = ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
            var responseCommand = ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));

            var data = await client.StreamCommunication.ReadAsync(ProtocolConstants.IntegerTypeLength);
            var dataLength = ConversionHandler.ConvertBytesToInt(data);

            var result = new List<Photo>();
            while (dataLength != 0)
            {
                var email = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(User.UserEmailLength));
                var photoId = ConversionHandler.ConvertBytesToLong(await client.StreamCommunication.ReadAsync(ProtocolConstants.LongTypeLength));
                var photoName = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(Photo.PhotoNameLength));
                var extension = ConversionHandler.ConvertBytesToString(await client.StreamCommunication.ReadAsync(Photo.PhotoExtensionLength));
                var photoLength = ConversionHandler.ConvertBytesToLong(await client.StreamCommunication.ReadAsync(ProtocolConstants.LongTypeLength));

                dataLength -= User.UserEmailLength + ProtocolConstants.LongTypeLength + Photo.PhotoNameLength + Photo.PhotoExtensionLength + ProtocolConstants.LongTypeLength;
                result.Add(new Photo()
                {
                    User = new User()
                    {
                        Email = email
                    },
                    Id = photoId,
                    Name = photoName,
                    FileSize = photoLength,
                    Extension = extension,
                });
            }
            
            return result;
        }

    }
}