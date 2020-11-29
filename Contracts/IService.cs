using System.Collections.Generic;
using Domain;
using TCPComm;

namespace Contracts
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
