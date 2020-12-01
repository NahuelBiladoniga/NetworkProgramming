using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using TCPComm.Protocol;
using Contracts;
using TCPComm;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Server
{
    public class Server
    {
        public IService Service { get; }
        private TcpListener _listener;
        public bool AcceptClients { get; set; }
        public User UserLogged { get; set; }
        
        public Server(TcpListener listener, IService service)
        {
            _listener = listener;
            Service = service;

            AcceptClients = true;

            AcceptConnections();
        }

        private void AcceptConnections()
        {
            while (AcceptClients)
            {
                AddClient();
            }
        }

        private void AddClient()
        {
            var client = new CommunicationClient(_listener.AcceptTcpClient());
            // Service.AddClient(client);
            
            new Thread(() => Connection(client)).Start();
        }

        private void Connection(CommunicationClient client)
        {
            try
            {
                GetData(client);
            }
            catch (SocketException)
            {
                DisconnectUser(client);
                client.ClientListener.Close();
            }
        }

        private async void GetData(CommunicationClient client)
        {
            ValidateLogin(client);
            
            var gettingData = true;
            
            while (gettingData)
            {
                await ProcessCommands(client);
            }
        }

        private async Task ProcessCommands(CommunicationClient client)
        {
            var command = await client.StreamCommunication.RecieveDataString();
            var infoSplitter = command.Split('$');
            var bodydata = infoSplitter.Skip(1).ToArray();
            switch (infoSplitter[0])
            {
                case "CreateUser":
                    ClientHandler.HandleCreateUser(bodydata, this,client);
                    break;
                case "UploadPhoto":
                    ClientHandler.HandleUploadPhoto(bodydata, this,client);
                    break;
                case "ViewUsers":
                    ClientHandler.HandleViewUsers(bodydata, this,client);
                    break;
                case "ViewPhotos":
                    ClientHandler.HandleViewPhotos(bodydata, this,client);
                    break;
                case "ViewCommentsPhoto":
                    ClientHandler.HandleViewCommentsPhoto(bodydata, this,client);
                    break;
                case "CommentPhoto":
                    ClientHandler.HandleCommentPhoto(bodydata, this,client);
                    break;
                default:
                    client.StreamCommunication.SendDataString("Error$Message=Invalid Command");
                    break;
            }
        }

        private async void ValidateLogin(CommunicationClient client)
        {
            var existUser = false;
            do
            {
                var command = await client.StreamCommunication.RecieveDataString();
                var infoSplitter = command.Split('$');
                var user = new User(infoSplitter);
                existUser = Service.AutenticateUser(user);
                if (!existUser)
                {
                    client.StreamCommunication.SendDataString("Error$Message=Invalid user");
                }
                else
                {
                    client.StreamCommunication.SendDataString("Ok$Message=Login Successfully");
                }
            } while (!existUser);
        }

        private void DisconnectUser(CommunicationClient client)
        {
            // Console.WriteLine("\n\nSe ha desconectado: ", client.ToString());
            // Service.DisconnectUser(client);
        }

        public string[] GetConnectedClients()
        {
            var connectedClients = new List<string>();
            Service.GetAllClients().ForEach(c => connectedClients.Add(c.ToString()));
            return connectedClients.ToArray();
        }    
    }
}