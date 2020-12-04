using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryClient.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LastConnection { get; set; }
        public bool IsLogedIn { get; set; }
    }
}
