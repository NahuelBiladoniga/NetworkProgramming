using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using TCPComm.Protocol;
using Contracts;
using TCPComm;
using System.Linq;
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
            this.Service = service;

            AcceptClients = true;

            new Thread(AcceptConnections).Start();

            //server.Stop();
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
            CommunicationClient client = new CommunicationClient(_listener.AcceptTcpClient());
            Service.AddClient(client);
            
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
                client.clientListener.Close();
            }
        }

        private void GetData(CommunicationClient client)
        {
            ValidateLogin(client);
            //while (gettingData)
            //{
            //    ExtendedProtocol protocol = new ExtendedProtocol(client.Socket, new CustomFileStream());
            //    string command = protocol.RecieveDataString();
            //    //_service.ExecuteOperation(command, protocol);
            //}
        }
        private async void ValidateLogin(CommunicationClient client)
        {
            bool existUser = false;
            do
            {
                string command = await client.networkCommunication.RecieveDataString();
                var infoSplitter = command.Split('$');
                var user = new User(infoSplitter);
                existUser = Service.AutenticateUser(user);
                if (!existUser)
                {
                    await client.networkCommunication.SendDataString("Error$Mensage={message}");
                }
                else
                {
                    await client.networkCommunication.SendDataString("Ok$Mensage={message}");
                }
            } while (!existUser);


        }

        private void DisconnectUser(CommunicationClient client)
        {
            Console.WriteLine("\n\nSe ha desconectado: ", client.ToString());
            // Service.DisconnectUser(client);
        }

        public string[] GetConnectedClients()
        {
            List<string> connectedClients = new List<string>();
            Service.GetAllClients().ForEach(c => connectedClients.Add(c.ToString()));
            return connectedClients.ToArray();
        }    
    }
}
