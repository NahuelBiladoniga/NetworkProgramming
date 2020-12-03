using System.Collections.Generic;
using Domain;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        public User GetUser(User user);
        User SaveUser(User user);
        User UpdateUser(User user);
        void DeleteUser(User user);
        int GetTotalUsers();
    }
}
