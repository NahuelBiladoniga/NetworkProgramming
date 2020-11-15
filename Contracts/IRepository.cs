using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Contracts
{
    public interface IRepository
    {
        List<Client> GetClients();

        void AddClient(Client client);

        bool DeleteClient(Client client);
    }
}
