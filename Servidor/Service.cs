using System;
using System.Collections.Generic;
using Services.Interfaces;
using Domain;
using Repositories.Interfaces;
using TCPComm;
using Repositories;

namespace Servidor
{
    public class Service : IService
    {
        private IRepository _repository;
        private Repository repository;

        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public Service(Repository repository)
        {
            this.repository = repository;
        }

        public List<User> GetAllClients()
        {
            return _repository.GetUsers();
        }

        public void AddUser(User client)
        {
            _repository.AddUser(client);
        }

        public void DeleteUser(User client)
        {
            _repository.DeleteUser(client);
        }

        public int GetClientSize(){
            return _repository.GetUsers().Count;
        }

        public bool ContainsUser(User user)
        {
            return _repository.GetUsers().Contains(user);
        }

        public void AddClient(CommunicationClient client)
        {
            throw new NotImplementedException();
        }
    }
}
