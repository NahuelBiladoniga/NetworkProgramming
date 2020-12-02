using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.IRemoteServers
{
    public interface ILoginRemoteServer
    {
        string Login(string email, string password);
    }
}
