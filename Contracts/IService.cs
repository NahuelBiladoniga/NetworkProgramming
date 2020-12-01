using System.Collections.Generic;
using Domain;
using TCPComm;

namespace Contracts
{
    public interface IService
    {
        List<User> GetAllClients();
        List<User> GetConnectedClients();
        List<Photo> GetPhotosFromUser(User user);
        List<Comment> GetCommentsFromPhoto(Photo id);
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        void DisconnectUser(User user);
        int GetClientSize();
        void AddClient(CommunicationClient client);
        bool ContainsUser(User user);
        bool AutenticateUser(User user);
        void CommentPhoto(Comment comment);
        void UploadPhoto(Photo photo);
    }
}
