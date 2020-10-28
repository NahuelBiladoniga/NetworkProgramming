using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPComm.Protocol;

namespace Controller.FileClient
{
    public class FileClient
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting client...");
            var clientIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
            TcpClient client = new TcpClient(clientIpEndPoint);
            Console.WriteLine("Attempting connection to server...");
            await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 6000);
            NetworkStream stream = client.GetStream();
            var communication = new NetworkCommunication(stream);
            var readTask = Task.Run(async () => await ReadAsync(communication));
            await WriteAsync(communication);
        }

        static async Task WriteAsync(NetworkCommunication communication)
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
