using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPComm.Protocol
{
    public class StreamCommunication
    {
        public const int FixedNameSize = 4;
        public const int FixedFileSize = 8;
        public const int MaxPacketSize = 32768;
        private readonly NetworkStream _networkStream;

        public StreamCommunication(NetworkStream networkStream)
        {
            _networkStream = networkStream;
        }
        
        public static long CalculateFileParts(long fileSize)
        {
            var fileParts = fileSize / MaxPacketSize;
            return fileParts * MaxPacketSize == fileSize ? fileParts : fileParts + 1;
        }

        public void Write(byte[] data)
        {
            var dataRead = 0;
            var length = data.Length;
            _networkStream.Write(buffer: data,
                                offset: dataRead,
                                size: length - dataRead);
        }

        public async Task<byte[]> ReadAsync(int length)
        {
            var offset = 0;
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
    }
}
