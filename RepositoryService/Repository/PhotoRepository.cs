using System.Collections.Generic;
using System.Linq;
using Domain;
using Repositories.Interfaces;

namespace Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private object lock_photo = new object();

        public void CommentPhoto(Comment commentEntity)
        {
            throw new System.NotImplementedException();
        }

        public void UploadPhoto(User user, Photo photo)
        {
            lock (lock_photo)
            {
                photo.UpdateId();
                var userListed = Repository.Users.Find(u => u.Equals(user));
                userListed.Photos.Add(photo);
            }
        }

        public List<Photo> GetPhotos()
        {
            return Repository.Users.SelectMany(m => m.Photos).ToList();
        }
    }
}