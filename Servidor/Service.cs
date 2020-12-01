using System;
using System.Collections.Generic;
using Contracts;
using Domain;
using TCPComm;

namespace Server
{
    public class Service : IService
    {
        private IRepository _repository;
        
        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public List<User> GetAllClients()
        {
            return _repository.GetUsers();
        }

        public List<User> GetConnectedClients()
        {
            throw new NotImplementedException();
        }

        public List<Photo> GetPhotosFromUser(User user)
        {
            return _repository.GetPhotosFromUser(user);
        }

        public List<Comment> GetCommentsFromPhoto(Photo id)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            _repository.AddUser(user);
        }

        public void DeleteUser(User user)
        {
            _repository.DeleteUser(user);
        }

        public void UpdateUser(User user)
        {
            _repository.UpdateUser(user);
        }

        public void DisconnectUser(User user)
        {
            _repository.DisconnectUser(user);
        }

        public int GetClientSize(){
            return _repository.GetUsers().Count;
        }

        public void AddClient(CommunicationClient client)
        {
            throw new NotImplementedException();
        }

        public bool ContainsUser(User user)
        {
            return _repository.GetUsers().Contains(user);
        }

        public bool AutenticateUser(User user)
        {
            var savedUser = _repository.GetUser(user);
            return savedUser.IsUserValid(user);
        }
        
        public void CommentPhoto(Comment comment)
        {
            _repository.CommentPhoto(comment);
        }

        public void UploadPhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
