using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using RabbitMQ.Client;
using TCPComm;
using TCPComm.Protocol;

namespace FileServer
{
    public class Server
    {
        public RepositoryClient.RepositoryHandler Service { get; }
        private readonly TcpListener _listener;
        public bool AcceptClients { get; set; }
        public User UserLogged { get; set; }
        
        public Server(TcpListener listener)
        {
            _listener = listener;
            Service = new RepositoryClient.RepositoryHandler();

            AcceptClients = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            AcceptConnections();
        }

        public void SendMessages(string logToSend)
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            QueueDeclare(channel);
            PublishMessage(logToSend, channel);
        }

        private void QueueDeclare(IModel channel)
        {
            channel.QueueDeclare(queue: "QueueForLogs", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishMessage(string message, IModel channel)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: string.Empty, routingKey: "QueueForLogs", basicProperties: null, body: body);
            }
        }

        private void AcceptConnections()
        {
            new Thread(() => {
            while (AcceptClients)
            {
                    SendMessages("ATR PRRO FCUMBIA CAGETEALA PIOLA GATOO");
                    AddClient();
            }
            
            }).Start();
            
        }

        private void AddClient()
        {
            var client = new CommunicationClient(_listener.AcceptTcpClient());
            
            new Thread(async () => await Connection(client)).Start();
        }

        private async Task Connection(CommunicationClient client)
        {
            try
            {
                await GetData(client);
            }
            catch (Exception)
            {
                DisconnectUser(client);
                client.ClientListener.Close();
            } 
        }

        private async Task GetData(CommunicationClient client)
        {
            var request = ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
            var commandType = ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));

            if(commandType == (short)ProtocolConstants.RequestCommands.LOGIN)
            {            
                await ClientHandler.ValidateLogin(this,client);
            }else 
            {
                await ClientHandler.HandleCreateUser(this, client);
            }

            var gettingData = true;
            
            while (gettingData)
            {
                await ProcessCommands(client);
            }
        }

        private async Task ProcessCommands(CommunicationClient client)
        {
            var request = ConversionHandler.ConvertBytesToShort( await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
            if (request == (short)ProtocolConstants.Commands.REQUEST )
            {
                var commandType = ConversionHandler.ConvertBytesToShort( await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
                
                switch (commandType)
                {
                    case (short) ProtocolConstants.RequestCommands.LOGIN:
                        await ClientHandler.HandleCreateUser( this,client);
                        break;
                    case (short) ProtocolConstants.RequestCommands.UPLOAD_PHOTO:
                        await ClientHandler.HandleUploadPhoto( this,client);
                        break;
                    case (short) ProtocolConstants.RequestCommands.VIEW_USERS:
                        ClientHandler.HandleViewUsers( this,client);
                        break;
                    case (short) ProtocolConstants.RequestCommands.VIEW_PHOTOS:
                        ClientHandler.HandleViewPhotos( this,client);
                        break;
                    case (short) ProtocolConstants.RequestCommands.VIEW_COMMENTS:
                        await ClientHandler.HandleViewCommentsPhoto( this,client);
                        break;
                    case (short) ProtocolConstants.RequestCommands.COMMENT_PHOTO:
                        await ClientHandler.HandleCommentPhoto(this,client);
                        break;
                    default:
                        ProtocolHelpers.SendResponseCommand(ProtocolConstants.ResponseCommands.Error ,client.StreamCommunication);
                        client.StreamCommunication.Write(ConversionHandler.ConvertStringToBytes("Invalid User", ProtocolConstants.ResponseMessageLength));
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

        public async Task<string[]> GetConnectedClients()
        {
            // var connectedClients = new List<string>();
            // (await Service.ViewUsers()).ForEach(c => connectedClients.Add(c.ToString()));
            // return connectedClients.ToArray();
            return (new List<string>()).ToArray();
        }    
    }
}