using System;
using System.Data;
using Domain;

namespace TCPComm.Protocol
{
    public static class ProtocolHelpers
    {
        public static void SendResponseCommand(ProtocolConstants.ResponseCommands command,StreamCommunication streamCommunication)
        {
            var requestData = ConversionHandler.ConvertCommandToByte(ProtocolConstants.Commands.Response);
            var commandData = ConversionHandler.ConvertResponseCommandToByte(command);

            streamCommunication.Write(requestData);
            streamCommunication.Write(commandData);
        }
        
        // public static void SendResponseCommand(StreamCommunication streamCommunication,string message)
        // {
        //     var requestData = ConversionHandler.ConvertCommandToByte(ProtocolConstants.Commands.Response);
        //     var commandData = ConversionHandler.ConvertResponseCommandToByte(ProtocolConstants.ResponseCommands.Ok);
        //     var messageData = ConversionHandler.ConvertStringToBytes(message);
        //
        //     streamCommunication.Write(requestData);
        //     streamCommunication.Write(commandData);
        //     streamCommunication.Write(messageData);
        // }
        
        public static void SendUserData(StreamCommunication streamCommunication,User user)
        {
            var nameData = ConversionHandler.ConvertStringToBytes(user.Name);
            var emailData = ConversionHandler.ConvertStringToBytes(user.Email);
            var lastConnectionData = ConversionHandler.ConvertStringToBytes(user.LastConnection.ToString());

            streamCommunication.Write(nameData);
            streamCommunication.Write(emailData);
            streamCommunication.Write(lastConnectionData);
        }
        
        public static void SendCommentData(StreamCommunication streamCommunication,Comment comment)
        {
            var nameData = ConversionHandler.ConvertStringToBytes(comment.Commentor.Name);
            var emailData = ConversionHandler.ConvertStringToBytes(comment.Commentor.Email);
            var commentData = ConversionHandler.ConvertStringToBytes(comment.Message);

            streamCommunication.Write(nameData);
            streamCommunication.Write(emailData);
            streamCommunication.Write(commentData);
        }
        
        public static void SendPhotoData(StreamCommunication streamCommunication,Photo photo)
        {
            var nameData = ConversionHandler.ConvertStringToBytes(comment.Commentor.Name);
            var extData = ConversionHandler.ConvertStringToBytes(comment.Commentor.Email);
            var sizeData = ConversionHandler.ConvertIntToBytes(photo.);

            streamCommunication.Write(nameData);
            streamCommunication.Write(emailData);
            streamCommunication.Write(commentData);
        }
    }
}