using System;
using System.Data;
using System.Threading.Tasks;
using Domain;
using TCPComm.Dto;

namespace TCPComm.Protocol
{
    public static class ProtocolHelpers
    {
        public static void SendResponseCommand(ProtocolConstants.ResponseCommands command,StreamCommunication streamCommunication)
        {
            var requestData = ConversionHandler.ConvertCommandToByte(ProtocolConstants.Commands.RESPONSE);
            var commandData = ConversionHandler.ConvertResponseCommandToByte(command);

            streamCommunication.Write(requestData);
            streamCommunication.Write(commandData);
        }
        
        public static void SendRequestCommand(ProtocolConstants.RequestCommands command,StreamCommunication streamCommunication)
        {
            var requestData = ConversionHandler.ConvertCommandToByte(ProtocolConstants.Commands.REQUEST);
            var commandData = ConversionHandler.ConvertRequestCommandToByte(command);

            streamCommunication.Write(requestData);
            streamCommunication.Write(commandData);
        }

        public static void SendMessageCommand(ProtocolConstants.ResponseCommands response, CommunicationClient client, string message)
        {
            SendResponseCommand(response, client.StreamCommunication);
            client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes(message, ProtocolConstants.ResponseMessageLength));
        }

        public static async Task<MessageResponse> RecieveMessageCommand(StreamCommunication streamCommunication)
        {
            var response = ConversionHandler.ConvertBytesToShort(await streamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
            var responseCommand = ConversionHandler.ConvertBytesToShort(await streamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
            var responseMessage = ConversionHandler.ConvertBytesToString(await streamCommunication.ReadAsync(ProtocolConstants.ResponseMessageLength));
            
            return new MessageResponse()
            {
                Message = responseMessage,
                responseCommands = (ProtocolConstants.ResponseCommands) responseCommand
            };
        }
        
        public static void SendUserData(StreamCommunication streamCommunication,User user)
        {
            var nameData = ConversionHandler.ConvertStringToBytes(user.Name, User.UserNameLength);
            var emailData = ConversionHandler.ConvertStringToBytes(user.Email, User.UserEmailLength);
            var lastConnectionData = ConversionHandler.ConvertStringToBytes(user.LastConnection.ToString("dd-MM-yyyy HH:mm"), ProtocolConstants.DateTimeTypeLength);

            streamCommunication.Write(nameData);
            streamCommunication.Write(emailData);
            streamCommunication.Write(lastConnectionData);
        }
        
        public static void SendCommentData(StreamCommunication streamCommunication,Comment comment)
        {
            var nameData = ConversionHandler.ConvertStringToBytes(comment.Commentator.Name, User.UserNameLength);
            var emailData = ConversionHandler.ConvertStringToBytes(comment.Commentator.Email, User.UserEmailLength);
            var commentData = ConversionHandler.ConvertStringToBytes(comment.Message, Comment.CommentLength);

            streamCommunication.Write(nameData);
            streamCommunication.Write(emailData);
            streamCommunication.Write(commentData);
        }
        
        public static void SendPhotoData(StreamCommunication streamCommunication,Photo photo)
        {
            var email = ConversionHandler.ConvertStringToBytes(photo.User.Email, User.UserEmailLength);
            var photoId = ConversionHandler.ConvertLongToBytes(photo.Id);
            var nameData = ConversionHandler.ConvertStringToBytes(photo.Name, Photo.PhotoNameLength);
            var extData = ConversionHandler.ConvertStringToBytes(photo.Extension, Photo.PhotoExtensionLength);
            var sizeData = ConversionHandler.ConvertLongToBytes(photo.FileSize);
            
            streamCommunication.Write(email);
            streamCommunication.Write(photoId);
            streamCommunication.Write(nameData);
            streamCommunication.Write(extData);
            streamCommunication.Write(sizeData);
        }
        
        public static long CalculateFileParts(long fileSize)
        {
            var fileParts = fileSize / ProtocolConstants.MaxPacketSize;
            return fileParts * ProtocolConstants.MaxPacketSize == fileSize ? fileParts : fileParts + 1;
        }
    }
}