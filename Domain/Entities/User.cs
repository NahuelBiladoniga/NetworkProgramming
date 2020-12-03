using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Domain
{
    public class User
    {
        public const int UserNameLength = 40;
        public const int UserEmailLength = 40;
        public const int UserPasswordLength = 40;

        public string Name { get; set; }
        public string Email { get; set; }        
        public string Password { get; set; }
        public DateTime LastConnection { get; set; }

        public User()
        {
        }

        public User(Socket socket)
        {
            LastConnection = DateTime.Now;
        }


        public User(string[] data)
        {
            string name = Array.Find(data, d => d.StartsWith("Name="));
            string description = Array.Find(data, d => d.StartsWith("Email="));

            Name = name != null ? name.Split('=')[1] : string.Empty;
            Email = description != null ? description.Split('=')[1] : string.Empty;
        }

        public List<Photo> Photos { get; set; }
        public bool IsConnected { get; set; }
        
        public override string ToString() => $"Nombre: {Name} - Email: {Email} - Fecha ultima conexion: {LastConnection.ToString("MM/dd/yy H:mm")}";
        
        public override bool Equals(object obj)
        {
            User other = obj as User;
            return other != null && (Email == other.Email);
        }

        public bool IsUserValid(User user)
        {
            return Email.Equals(user.Email) && Password.Equals(user.Password);
        }

        public override int GetHashCode()
        {
            return -9567636 + EqualityComparer<String>.Default.GetHashCode(Email);
        }
    }
}
