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

namespace Servidor
{
    public class Client
    {
        public IService Service { get; }
        private TcpListener _listener;
        public bool AcceptClients { get; set; }

        public Client(TcpListener listener, IService service)
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
            TCPComm.CommunicationClient client = new TCPComm.CommunicationClient(_listener.AcceptTcpClient());
            Service.AddClient(client);
            new Thread(() => Connection(client)).Start();
        }

        private void Connection(TCPComm.CommunicationClient client)
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

        private async void GetData(TCPComm.CommunicationClient client)
        {
            ValidateLogin(client);
            //while (gettingData)
            //{
            //    ExtendedProtocol protocol = new ExtendedProtocol(client.Socket, new CustomFileStream());
            //    string command = protocol.RecieveDataString();
            //    //_service.ExecuteOperation(command, protocol);
            //}
        }
        private async void ValidateLogin(TCPComm.CommunicationClient client)
        {
            bool existUser = false;
            do
            {
                string command = await client.networkCommunication.RecieveDataString();
                var infoSplitter = command.Split('$');
                var user = new User(infoSplitter);
                existUser = Service.ContainsUser(user);
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

        private void DisconnectUser(TCPComm.CommunicationClient client)
        {
            Console.WriteLine("\n\nSe ha desconectado: ", client.ToString());
            Service.DeleteClient(client);
        }

        public string[] GetConnectedClients()
        {
            List<string> connectedClients = new List<string>();
            Service.GetAllClients().ForEach(c => connectedClients.Add(c.ToString()));
            return connectedClients.ToArray();
        }    
    }
}
