using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace RepositoryServer
{
    public class UserService : Users.UsersBase
    {
        public override async Task<MessageReply> CreateUser(CreateUserInput request, ServerCallContext context)
        {
            return await Task.FromResult(new MessageReply
            {
                Message = "Test",
                Code = 2
            });
        }
    }
}
