using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Domain;
using TCPComm;
using TCPComm.Protocol;

namespace TCPComm
{
    public class CommunicationClient
    {
        public TcpClient ClientListener { get; }
        public User User { get; set; }
        public readonly StreamCommunication StreamCommunication;

        public CommunicationClient(TcpClient clientListener)
        {
            ClientListener = clientListener;
            StreamCommunication = new StreamCommunication(clientListener.GetStream());
        }
    }
}