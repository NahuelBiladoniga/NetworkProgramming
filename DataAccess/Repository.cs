using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Contracts;


namespace DataAccess
{
    public class Repository : IRepository
    {
        public static List<User> Users { get; set; }

        private object lock_users = new object();

        public Repository()
        {
            Users = new List<User>();
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public void AddUser(User user)
        {
            lock (lock_users)
            {
                Users.Add(user);
            }
        }

        public void UpdateUser(User user)
        {
            lock (lock_users)
            {
                try
                {
                    User old_user = Users.Find(c => c.Email == user.Email);
                    old_user.Name = user.Name;
                    old_user.Email = user.Email;
                }
                catch (ArgumentNullException)
                {
                    throw new DataAccessException($"Error al editar cliente. El cliente {user.ToString()} no existe.");
                }
            }
        }

        public bool DeleteUser(User user)
        {
            lock (lock_users)
            {
                return Users.Remove(user);
            }
        }
    }

    
}
