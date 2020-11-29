using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using TCPComm;
using TCPComm.Protocol;

namespace TCPComm
{
    public class CommunicationClient
    {
        public TcpClient clientListener { get; }

        public NetworkCommunication networkCommunication;

        public CommunicationClient(TcpClient clientListener)
        {
            this.clientListener = clientListener;
            this.networkCommunication = new NetworkCommunication(clientListener.GetStream());
        }
    }
}
