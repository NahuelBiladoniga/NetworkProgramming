using System.Collections.Generic;
using Domain;

namespace RepositoryService.Interfaces
{
    public interface ICommentsRepository
    {
        void CommentPhoto(Photo photo, Comment commentEntity);
        List<Comment> GetCommentsFromPhoto(Photo id);
    }
}