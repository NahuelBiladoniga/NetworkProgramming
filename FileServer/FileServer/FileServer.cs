using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPComm.Protocol;

namespace Controller.FileServer
{
    public class FileServer
    {
        static TcpListener server;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting server");
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
            server = new TcpListener(ipEndPoint);
            server.Start(100);

            while (true)
            {
                await WaitForClientConnection();
            }
        }

        static async Task WaitForClientConnection()
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();
            var communication = new NetworkCommunication(stream);
            var readTask = Task.Run(async () => await ReadAsync(communication));
            await WriteAsync(communication);
        }

        static async Task WriteAsync(NetworkCommunication communication, ProtocolResponse response)
        {
            while (true)
            {
                var msg = Console.ReadLine();
                var data = Encoding.UTF8.GetBytes(msg);
                byte[] dataLength = BitConverter.GetBytes(data.Length);
                await communication.WriteAsync(dataLength);
                await communication.WriteAsync(data);
            }
        }

        static async Task ReadAsync(NetworkCommunication communication)
        {
            while (true)
            {
                byte[] dataLength = await communication.ReadAsync(ProtocolConstants.FixedDataSize);
                int dataSize = BitConverter.ToInt32(dataLength, 0);
                byte[] data = await communication.ReadAsync(dataSize);
                var msg = Encoding.UTF8.GetString(data);
                Console.WriteLine(msg);
            }
        }
    }
}
