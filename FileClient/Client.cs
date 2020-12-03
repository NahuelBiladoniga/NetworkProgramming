using Domain;
using TCPComm.Protocol;

namespace FileClient
{
    public class Client
    {
        public StreamCommunication StreamCommunication { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        //public User User { get; set; }
    }
}
