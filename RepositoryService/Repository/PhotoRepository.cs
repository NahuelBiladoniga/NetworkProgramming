using System.Collections.Generic;
using System.Linq;
using Domain;
using RepositoryService.Interfaces;

namespace Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private object lock_photo = new object();
        private readonly Repository repository;

        public PhotoRepository()
        {
            repository = Repository.GetInstance;
        }

        public void UploadPhoto(User user, Photo photo)
        {
            lock (lock_photo)
            {
                photo.UpdateId();
                var userListed = repository.Users.Find(u => u.Equals(user));
                photo.User = userListed;
                userListed.Photos.Add(photo);
            }
        }

        public List<Photo> GetPhotos()
        {
            return repository.Users.SelectMany(m => m.Photos).ToList();
        }
    }
}