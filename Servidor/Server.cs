using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Contracts;
using TCPComm;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using TCPComm.Protocol;

namespace Server
{
    public class Server
    {
        public IService Service { get; }
        private readonly TcpListener _listener;
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

        private async Task GetData(CommunicationClient client)
        {
            await ClientHandler.ValidateLogin(this,client);
            
            var gettingData = true;
            
            while (gettingData)
            {
                await ProcessCommands(client);
            }
        }

        private async Task ProcessCommands(CommunicationClient client)
        {
            var request = ConversionHandler.ConvertBytesToInt( await client.StreamCommunication.ReadAsync(ProtocolConstants.RequestTypeLength));
            if (request == (int)ProtocolConstants.Commands.Request )
            {
                var commandType = ConversionHandler.ConvertBytesToInt( await client.StreamCommunication.ReadAsync(ProtocolConstants.CommandTypeLength));
            
                switch (commandType)
                {
                    case (int) ProtocolConstants.RequestCommands.Login:
                        await ClientHandler.HandleCreateUser( this,client);
                        break;
                    case (int) ProtocolConstants.RequestCommands.UploadPhoto:
                        await ClientHandler.HandleUploadPhoto( this,client);
                        break;
                    case (int) ProtocolConstants.RequestCommands.ViewUsers:
                        ClientHandler.HandleViewUsers( this,client);
                        break;
                    case (int) ProtocolConstants.RequestCommands.ViewPhotos:
                        await ClientHandler.HandleViewPhotos( this,client);
                        break;
                    case (int) ProtocolConstants.RequestCommands.ViewComments:
                        await ClientHandler.HandleViewCommentsPhoto( this,client);
                        break;
                    case (int) ProtocolConstants.RequestCommands.CommentPhoto:
                        await ClientHandler.HandleCommentPhoto(this,client);
                        break;
                    default:
                        ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                        client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Invalid Command"));
                        break;
                }
            }
            else
            {
                //TODO INVALID
            }
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