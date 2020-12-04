using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Comment
    {
        public const int CommentLength = 300;

        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public Photo Photo { get; set; }
        public User Commentator { get; set; }

        public override string ToString() => $"Nombre de usuario: {Commentator.Name} - Email de usuario: {Commentator.Email} - Mensaje: {Message}";

        public string ToStringProtocol() => $"Comment={Message}$CreateDate={CreationDate.ToLocalTime()}";
    }
}
