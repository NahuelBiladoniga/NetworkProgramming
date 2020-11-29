using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPComm.Protocol
{
    public class NetworkCommunication
    {
        private readonly NetworkStream _networkStream;

        public NetworkCommunication(NetworkStream networkStream)
        {
            _networkStream = networkStream;
        }

        public async Task WriteAsync(byte[] data)
        {
            int dataRead = 0;
            int length = data.Length;
            _networkStream.Write(buffer: data,
                                offset: dataRead,
                                size: length - dataRead);
            
        }

        public async Task<byte[]> ReadAsync(int length)
        {
            int offset = 0;
            var data = new byte[length];
            while (offset < length)
            {
                var read = await _networkStream.ReadAsync(data, offset, length - offset);
                if (read == 0)
                    throw new Exception("Connection lost");
                offset += read;
            }

            return data;
        }

        public async Task<string> RecieveDataString()
        {
            return Encoding.UTF8.GetString(await Task.Run(async () => await RecieveDataBytes()));
        }

        public async Task<int> ReadDataLengthAsync(){
            var data = await Task.Run(async () => await ReadAsync(ProtocolConstants.SIZE));
            return Utils.ByteToInt(data);            
        }

        public async Task<byte[]> RecieveDataBytes()
        {
            var dataLength = await Task.Run(async () => await ReadDataLengthAsync());
            var dataReceive = await Task.Run(async () => await ReadAsync(dataLength));
            return dataReceive;
        }

        public async Task SendDataString(String data)
        {
            await Task.Run(async () => await WriteAsync(Utils.StringToByte(data)));
        }

    }
}
