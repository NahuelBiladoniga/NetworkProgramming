using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Photo
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FileSize { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; }

        public override string ToString() => $"Nombre: {Name} - Extension: {Extension} - Tamanio: : {FileSize} - Nombre Usuario: {User.Name}";

        public override bool Equals(object obj)
        {
            Photo other = obj as Photo;
            return other != null && (Id == other.Id);
        }

    }
}
