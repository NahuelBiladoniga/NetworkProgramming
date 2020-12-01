using System;
using System.Collections.Generic;
using Domain;
using TCPComm;
using TCPComm.Protocol;

namespace Server
{
    public static class ClientHandler
    {
        private static readonly string PhotosPath = AppDomain.CurrentDomain.BaseDirectory + "Photos";

        public static void HandleCreateUser(string[] bodydata, Server server, CommunicationClient client)
        {
            var name = Array.Find(bodydata, d => d.StartsWith("Name="));
            var email = Array.Find(bodydata, d => d.StartsWith("Email="));

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                client.StreamCommunication.SendDataString("Error$Message=Invalid params");
            }

            var user = new User
            {
                Email = email,
                Name = name
            };
            
            server.Service.AddUser(user);
            
            client.StreamCommunication.SendDataString("Ok$Message=Create ");
        }

        public static void HandleUploadPhoto(string[] bodydata, Server server, CommunicationClient client)
        {
            var name = Array.Find(bodydata, d => d.StartsWith("Name="));
            var extension = Array.Find(bodydata, d => d.StartsWith("Ext="));
            var fileSize = Array.Find(bodydata, d => d.StartsWith("Size="));
            var parsedFileSize = (long)0;
            
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(extension) || long.TryParse(fileSize, out parsedFileSize))
            {
                client.StreamCommunication.SendDataString("Error$Message=Invalid params");
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
            
            client.FileCommsHandler.ReceiveFileWithStreams(parsedFileSize,fileName);
            
            client.StreamCommunication.SendDataString("Ok$Message=Photo uploaded");
        }

        public static void HandleViewUsers(string[] bodydata, Server server, CommunicationClient client)
        {
            var result = "";
            server.Service.GetAllClients().ForEach((elem) =>
            {
                result += elem.ToStringProtocol() + "\n";
            });
            
            client.StreamCommunication.SendDataString(result);
        }

        public static void HandleViewPhotos(string[] bodydata, Server server, CommunicationClient client)
        {
            var email = Array.Find(bodydata, d => d.StartsWith("Email="));

            if (string.IsNullOrWhiteSpace(email))
            {
                client.StreamCommunication.SendDataString("Error$Message=Invalid params");
            }

            var user = new User
            {
                Email = email
            };

            var result = "";
            server.Service.GetPhotosFromUser(user).ForEach((elem) =>
            {
                result += elem.ToStringProtocol() + "\n";
            });
            
            client.StreamCommunication.SendDataString(result);
        }

        public static void HandleCommentPhoto(string[] bodydata, Server server, CommunicationClient client)
        {
            var photoId = Array.Find(bodydata, d => d.StartsWith("PhotoId="));
            var message = Array.Find(bodydata, d => d.StartsWith("Comment="));

            if (string.IsNullOrWhiteSpace(message) || int.TryParse(photoId, out _))
            {
                client.StreamCommunication.SendDataString("Error$Message=Invalid params");
            }

            var comment = new Comment
            {
                Photo = new Photo
                {
                    Id = int.Parse(photoId)
                },
                Message = message,
                Commentor = client.User
            };
            
            server.Service.CommentPhoto(comment);
            
            client.StreamCommunication.SendDataString("Ok$Message=Comment Created");
        }

        public static void HandleViewCommentsPhoto(string[] bodydata, Server server, CommunicationClient client)
        {
            var photoId = Array.Find(bodydata, d => d.StartsWith("IdPhoto="));
            var photoIdParsed = (long)0;
            if (long.TryParse(photoId, out photoIdParsed))
            {
                client.StreamCommunication.SendDataString("Error$Message=Invalid params");
            }

            var photo = new Photo
            {
                Id = photoIdParsed
            };

            var result = "";
            server.Service.GetCommentsFromPhoto(photo).ForEach((elem) =>
            {
                result += elem.ToStringProtocol() + "\n";
            });
            
            client.StreamCommunication.SendDataString(result);

        }
    }
}