using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Photo
    {
        public const int IdSize = 20;
        public const int NameSize = 20;
        public const int LengthSize = 8;

        public string Id { get; set; }
        public object Name { get; set; }
        public int LengthFile { get; set; }
        public User User { get; set; }
    }
}
