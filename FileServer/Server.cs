using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using RabbitMQ.Client;
using RepositoryClient.Dto;
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

            AcceptConnections();
        }
        private void AcceptConnections()
        {
            new Thread(() =>
            {
                while (AcceptClients)
                {
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
                
                await DisconnectUserAsync(client);
                client.ClientListener.Close();
            } 
        }

        private async Task GetData(CommunicationClient client)
        {
            var logged = false;
            while(!logged)
            {
                var request = ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
                var commandType = ConversionHandler.ConvertBytesToShort(await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
                if (commandType == (short)ProtocolConstants.RequestCommands.LOGIN)
                {
                    logged = await ClientHandler.ValidateLogin(this, client);
                }
                else
                {
                    logged = await ClientHandler.HandleCreateUser(this, client);
                }
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
            var commandType = ConversionHandler.ConvertBytesToShort( await client.StreamCommunication.ReadAsync(ProtocolConstants.ShortTypeLength));
                
            switch (commandType)
            {
                case (short) ProtocolConstants.RequestCommands.LOGIN:
                    await ClientHandler.HandleCreateUser(this,client);
                    break;
                case (short) ProtocolConstants.RequestCommands.UPLOAD_PHOTO:
                    await ClientHandler.HandleUploadPhoto(this,client);
                    break;
                case (short) ProtocolConstants.RequestCommands.VIEW_USERS:
                    await ClientHandler.HandleViewUsers(this,client);
                    break;
                case (short) ProtocolConstants.RequestCommands.VIEW_PHOTOS:
                    await ClientHandler.HandleViewPhotos(this,client);
                    break;
                case (short) ProtocolConstants.RequestCommands.VIEW_COMMENTS:
                    await ClientHandler.HandleViewCommentsPhoto(this,client);
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

        private async Task DisconnectUserAsync(CommunicationClient client)
        {
            if(client.User != null)
            {
                var user = new UserDto()
                {
                    Email = client.User.Email
                };

                await Service.LogoutUser(user);
            }
        }

        public async Task<string[]> GetConnectedClients()
        {
            var users = await Service.GetAutenticatedUserAsync();
            var parsedUsers = users.Select((elem) => new User()
            {
                Email = elem.Email,
                Name = elem.Name,
                LastConnection = elem.LastConnection
            }.ToString()).ToArray();

            return parsedUsers;
        }    
    }
}