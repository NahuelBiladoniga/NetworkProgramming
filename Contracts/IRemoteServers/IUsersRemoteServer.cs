using Domain;
using System.Collections.Generic;

namespace Contracts
{
    public interface IUsersRemoteServer
    {
        IList<User> GetAllUsers();
        User GetUserById(string id);
        User AddUser(User user);
        bool ModifyUser(string id, User user);
        bool DeleteUser(string userId);
    }
}
