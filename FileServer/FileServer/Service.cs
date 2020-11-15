using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Domain;

namespace FileServer.FileServer
{
    class Service
    {
        private IRepository _repository;

        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public List<Client> GetClients()
        {
            return _repository.GetClients();
        }

        public void AddClient(Client client)
        {
            _repository.AddClient(client);
        }
    }
    
}
