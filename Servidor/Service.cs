﻿using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Domain;
namespace Servidor
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
    }
}
