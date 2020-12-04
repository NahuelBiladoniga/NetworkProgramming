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

        private static long _idCounter = 0;
        public long Id { get; set; } 
        public string Name { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();       
        public User User { get; set; }

        public override string ToString() => $"Id: {Id} - Nombre: {Name} - Extension: {Extension} - Tamanio: : {FileSize} - Nombre Usuario: {User.Name}";

        public string ToStringProtocol() => $"Name={Name}$Ext={Extension}$FileSize={FileSize}";

        public void UpdateId()
        {
            Id = _idCounter++;
        }
        
        public override bool Equals(object obj)
        {
            Photo other = obj as Photo;
            return other != null && (Id == other.Id);
        }
    }
}
