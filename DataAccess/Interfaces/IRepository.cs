using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Interfaces
{
    public interface IRepository
    {
        List<User> GetUsers();
        void AddUser(User user);
        bool DeleteUser(User user);
    }    
}
