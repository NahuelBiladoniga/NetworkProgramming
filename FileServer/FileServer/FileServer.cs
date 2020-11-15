using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Contracts;
using System.Threading;
using Domain;

namespace FileServer.FileServer
{
    class FileServer
    {
        private Socket _socket;
        private IService _service;

        public bool AcceptClients { get; set; }
        public FileServer(Socket socket, IService service)
        {
            _socket = socket;
            _service = service;

            AcceptClients = true;

            Thread acceptConnections = new Thread(AcceptConnections);
            acceptConnections.Start();
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
            Client client = new Client(_socket.Accept());
            _service.AddClient(client);
            Thread thread = new Thread(() => Connection(client));
            thread.Start();
        }

        private void Connection(Client client)
        {
            bool is_connected = true;
            try
            {
                GetInformation(is_connected, client);
            }
            catch (SocketException)
            {
                is_connected = false;
               //DESCONECTAR CLIENTE ()
                client.Socket.Shutdown(SocketShutdown.Both);
                client.Socket.Close();
            }
        }

        private void GetInformation(bool gettingData, Client client)
        {
           
        }
    }
}
