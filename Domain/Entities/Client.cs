using System;

namespace Domain
{
    public class Client
    {
        public const int NameSize = 20;
        public const int EmailSize = 20;
        public const int LastConnectionSize = 20;

        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime LastConnection { get; set; }
    }
}
