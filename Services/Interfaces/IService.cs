using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using TCPComm;

namespace Services.Interfaces
{
    public interface IService
    {
        List<User> GetAllClients();

        void AddUser(User client);

        void DeleteUser(User client);

        int GetClientSize();

        void AddClient(CommunicationClient client);

        bool ContainsUser(User user);
    }
}
