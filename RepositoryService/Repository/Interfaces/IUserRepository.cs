using System.Collections.Generic;
using Domain;

namespace RepositoryService.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetAutenticatedUsers();
        IEnumerable<User> GetUsersPaged(int offset, int size);
        public User GetUser(User user);
        User SaveUser(User user);
        User UpdateUser(User user);
        void DeleteUser(User user);
        bool ContainsUser(User user);
        int GetTotalUsers();
    }
}
