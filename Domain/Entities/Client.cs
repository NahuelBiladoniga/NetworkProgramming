using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Domain
{
    public class Client
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime LastConnection { get; set; }
        public Socket Socket { get; set; }

        public Client()
        {
        }

        public Client(Socket socket)
        {
            Socket = socket;
            LastConnection = DateTime.Now;
        }

        public override string ToString() => $"Cliente: {Socket.RemoteEndPoint} - Nombre: {Name} - Fecha ultima conexion: {LastConnection.ToString("MM/dd/yy H:mm")}";

        public override bool Equals(object obj)
        {
            Client other = obj as Client;
            return other != null ? (Socket == other.Socket) : false;
        }

        public override int GetHashCode()
        {
            return -9567636 + EqualityComparer<Socket>.Default.GetHashCode(Socket);
        }
    }
}
