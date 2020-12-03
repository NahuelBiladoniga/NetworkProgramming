using System.Collections.Generic;
using Domain;

namespace RepositoryService.Interfaces
{
    public interface IPhotoRepository
    {
        void UploadPhoto(User user, Photo photo);
        List<Photo> GetPhotos();
    }
}