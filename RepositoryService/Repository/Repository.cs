using System.Collections.Generic;
using Domain;

namespace Repositories
{
    public class Repository
    {
        private static readonly object lockPad = new object();

        Repository()
        {
        }
        private static Repository instance = null;
        public static Repository GetInstance
        {
            get
            {
                lock (lockPad)
                {
                    if (instance == null)
                    {
                        instance = new Repository();
                        instance.Users = new List<User>();
                    }
                    return instance;
                }
            }
        }

        public List<User> Users { get; set; }
    }
}
