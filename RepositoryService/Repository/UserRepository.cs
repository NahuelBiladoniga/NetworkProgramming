using Domain;
using RepositoryService.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly object lock_users = new object();
        private readonly Repository repository;

        public UserRepository()
        {
            repository = Repository.GetInstance;
        }

        public IEnumerable<User> GetUsers()
        {
            lock (lock_users)
            {
                return repository.Users;
            }
        }

        public User GetUser(User user)
        {
            lock (lock_users)
            {
                return repository.Users.First(u => u.Equals(user));
            }
        }

        public User SaveUser(User user)
        {
            lock (lock_users)
            {
                repository.Users.Add(user);
                return user;
            }
        }
        
        public User UpdateUser(User user)
        {
            lock (lock_users)
            {
                var userLocated = repository.Users.First(u => u.Equals(user));
                userLocated.Name = user.Name;
                userLocated.Password = user.Password;
            }

            return user;
        }

        public void DeleteUser(User user)
        {
            lock (lock_users)
            {
                repository.Users.Remove(user);
            }
        }

        public int GetTotalUsers()
        {
            lock (lock_users)
            {
                return repository.Users.Count();
            }
        }

        public IEnumerable<User> GetUsersPaged(int offset, int size)
        {
            return repository.Users.GetRange(offset,size);
        }

        public bool ContainsUser(User user)
        {
            return repository.Users.Contains(user);
        }
    }
}
