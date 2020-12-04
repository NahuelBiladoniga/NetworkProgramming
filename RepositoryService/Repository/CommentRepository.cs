using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using RepositoryService.Interfaces;

namespace Repositories
{
    public class CommentRepository : ICommentsRepository
    {
        private readonly Repository repository;
        private object lock_comment = new object();

        public CommentRepository()
        {
            repository = Repository.GetInstance;
        }

        public void CommentPhoto(Photo photo,Comment commentEntity)
        {
            lock (lock_comment)
            {
                var photos = repository.Users.SelectMany(u => u.Photos);
                var photoLinked = photos.First(p => p.Equals(photo));
                commentEntity.Photo = photoLinked;
                commentEntity.CreationDate = DateTime.Now;
                photoLinked.Comments.Add(commentEntity);
            }
        }

        public List<Comment> GetCommentsFromPhoto(Photo photo)
        {
            lock (lock_comment)
            {
                var user = repository.Users.Find(x => x.Photos.Contains(photo));
                var comments = user.Photos.Find(x => x.Equals(photo)).Comments.ToList();
                return comments;
            }
        }
    }
}