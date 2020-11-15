using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DataAccess
{
    class Repository
    {
        public static List<Client> Clients { get; set; }

        private object lock_clients = new object();

        public Repository()
        {
            Clients = new List<Client>();
        }

        public List<Client> GetClients()
        {
            return Clients;
        }

        public void AddClient(Client client)
        {
            lock (lock_clients)
            {
                Clients.Add(client);
            }
        }

        public bool DeleteClient(Client client)
        {
            lock (lock_clients)
            {
                return Clients.Remove(client);
            }
        }
    }
}
