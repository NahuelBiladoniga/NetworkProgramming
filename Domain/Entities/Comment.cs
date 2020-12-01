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
        public DateTime CreationDate { get; set; }

        public string ToStringProtocol() => $"Name={Commentor.Name}$Email={Commentor.Name}$Comment={Message}$CreateDate={CreationDate.ToLocalTime()}";
    }
}
