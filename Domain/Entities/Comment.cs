using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Comment
    {
        public string Message { get; set; }
        public Photo Photo { get; set; }
        public User Commentor { get; set; }

    }
}
