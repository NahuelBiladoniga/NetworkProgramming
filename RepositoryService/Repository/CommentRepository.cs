using System.Collections.Generic;
using System.Linq;
using Domain;
using Repositories.Interfaces;

namespace Repositories
{
    public class CommentRepository : ICommentsRepository

    {
        public void CommentPhoto(Photo photo,Comment commentEntity)
        {
            var photoLinked = Repository.Users.Find(x => x.Photos.Contains(photo)).Photos.First(x => x.Equals(photo));
            photoLinked.Comments.Add(commentEntity);
        }

        public List<Comment> GetCommentsFromPhoto(Photo photo)
        {
            return Repository.Users.Find(x => x.Photos.Contains(photo)).Photos.First(x => x.Equals(photo)).Comments;
        }
    }
}