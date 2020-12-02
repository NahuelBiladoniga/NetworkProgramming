using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace Domain
{
    public class Photo
    {
        public const int PhotoExtensionLength = 10;
        public const int PhotoNameLength = 40;

        private static long IdCounter = 0;
        public long Id { get; set; } 
        public string Name { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; }

        public override string ToString() => $"Nombre: {Name} - Extension: {Extension} - Tamanio: : {FileSize} - Nombre Usuario: {User.Name}";

        public string ToStringProtocol() => $"Name={Name}$Ext={Extension}$FileSize={FileSize}";

        public void UpdateId()
        {
            Id = IdCounter++;
        }
        
        public override bool Equals(object obj)
        {
            Photo other = obj as Photo;
            return other != null && (Id == other.Id);
        }

    }
}
