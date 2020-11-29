using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Contracts
{
    public interface IRepository
    {
        List<User> GetUsers();

        void AddUser(User user);

        bool DeleteUser(User user);
    }
}
