using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Contracts
{
    public interface IRepository
    {
        List<User> GetUsers();

        void AddUser(User user);

        bool DeleteUser(User user);
        List<Photo> GetPhotosFromUser(User user);
        void UpdateUser(User user);
        void DisconnectUser(User user);
        User GetUser(User user);
        void CommentPhoto(Comment commentEntity);
        void UploadPhoto(Photo photo);
        List<Comment> GetCommentsFromPhoto(Photo id);
        List<Photo> GetPhotos();
    }
}
