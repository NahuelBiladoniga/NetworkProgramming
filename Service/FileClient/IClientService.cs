using Domain;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Service.FileClient
{
    public interface IClientService
    {
        bool Login(string email);
        void CreateUser(User user);
        void DeleteUser(string email);
        IEnumerable<Photo> ListPhotos(string email = "");
        void UploadPhoto(Photo photo);
        void CommentPhoto(Comment comment);
        IEnumerable<Comment> ListCommentsFromPhoto(Photo photo);
    }
}
