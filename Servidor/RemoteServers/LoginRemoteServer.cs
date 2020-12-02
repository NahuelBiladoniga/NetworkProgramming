using Contracts.IRemoteServers;
using Contracts.IServices;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.RemoteServers
{
    class LoginRemoteServer : MarshalByRefObject, ILoginRemoteServer
    {
        ILoginService _loginService;

        public LoginRemoteServer(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public string Login(string email, string password)
        {
            Logger.LogMessage(new Log(System.Reflection.MethodBase.GetCurrentMethod().Name, string.Empty, string.Format("{0} is trying to login", email), LogPriority.Low, LogType.Information));
            return _loginService.CreateSession(email, password);
        }
    }
}
