using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Contracts;


namespace DataAccess
{
    public class Repository : IRepository
    {
        public static List<User> Users { get; set; } = new List<User>(); 
        public static List<Photo> Photos { get; set; } = new List<Photo>();
        public static List<Comment> Comments { get; set; } = new List<Comment>();

        private object lock_users = new object();

        public List<User> GetUsers()
        {
            return Users;
        }

        public void AddUser(User user)
        {
            lock (lock_users)
            {
                Users.Add(user);
            }
        }

        public List<Photo> GetPhotos()
        {
            return Photos.ToList();
        }

        public List<Photo> GetPhotosFromUser(User user)
        {
            return Photos.FindAll(p => p.User.Equals(user));
        }

        public void UpdateUser(User user)
        {
            lock (lock_users)
            {
                try
                {
                    User old_user = Users.Find(c => c.Email == user.Email);
                    old_user.Name = user.Name;
                    old_user.Email = user.Email;
                }
                catch (ArgumentNullException)
                {
                    throw new DataAccessException($"Error al editar cliente. El cliente {user.ToString()} no existe.");
                }
            }
        }

        public void DisconnectUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(User user)
        {
            return Users.Find(u => u.Equals(user));
        }

        public void CommentPhoto(Comment commentEntity)
        {
            var photo = Photos.Find(p => p.User.Equals(commentEntity.Commentor));
            commentEntity.Photo = photo;
            Comments.Add(commentEntity);
        }

        public bool DeleteUser(User user)
        {
            lock (lock_users)
            {
                return Users.Remove(user);
            }
        }

        public void UploadPhoto(Photo photo)
        {
            photo.UpdateId();
            var user = Users.Find(u => u.Equals(photo.User));
            photo.User = user;
            Photos.Add(photo);
        }

        public List<Comment> GetCommentsFromPhoto(Photo photo)
        {
            return Photos.Find(p => p.Equals(photo)).Comments;
        }
    }

    
}
