using Domain;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private object lock_users = new object();

        public IEnumerable<User> GetUsers()
        {
            lock (lock_users)
            {
                return Repository.Users;
            }
        }

        public User GetUser(User user)
        {
            lock (lock_users)
            {
                return Repository.Users.First(u => u.Equals(user));
            }
        }

        public User SaveUser(User user)
        {
            lock (lock_users)
            {
                Repository.Users.Add(user);
                return user;
            }
        }
        
        public User UpdateUser(User user)
        {
            lock (lock_users)
            {
                var userLocated = Repository.Users.First(u => u.Equals(user));
                userLocated.Name = user.Name;
                userLocated.Password = user.Password;
            }

            return user;
        }

        public void DeleteUser(User user)
        {
            lock (lock_users)
            {
                Repository.Users.Remove(user);
            }
        }

        public int GetTotalUsers()
        {
            lock (lock_users)
            {
                return Repository.Users.Count();
            }
        }
    }
}
