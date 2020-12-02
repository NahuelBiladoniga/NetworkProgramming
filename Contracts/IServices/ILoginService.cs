using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.IServices
{
    public interface ILoginService
    {
        User LoggedUser { get; }
        string CreateSession(string email, string password);
    }
}
