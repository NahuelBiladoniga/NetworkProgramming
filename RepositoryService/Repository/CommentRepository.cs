using System.Collections.Generic;
using System.Linq;
using Domain;
using RepositoryService.Interfaces;

namespace Repositories
{
    public class CommentRepository : ICommentsRepository
    {
        private readonly Repository repository;

        public CommentRepository()
        {
            repository = Repository.Instance;
        }

        public void CommentPhoto(Photo photo,Comment commentEntity)
        {
            var photoLinked = repository.Users.Find(x => x.Photos.Contains(photo)).Photos.First(x => x.Equals(photo));
            photoLinked.Comments.Add(commentEntity);
        }

        public List<Comment> GetCommentsFromPhoto(Photo photo)
        {
            return repository.Users.Find(x => x.Photos.Contains(photo)).Photos.First(x => x.Equals(photo)).Comments;
        }
    }
}