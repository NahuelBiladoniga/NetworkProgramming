
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TCPComm.Protocol
{
    public class FileCommsHandler
    {
        private readonly StreamCommunication _streamHandler;

        public FileCommsHandler(NetworkStream networkStream)
        {
           _streamHandler = new StreamCommunication(networkStream);
        }
        
        public void SendFileWithStream(long fileSize, string path)
        {
            long fileParts = StreamCommunication.CalculateFileParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == fileParts)
                {
                    var lastPartSize = (int) (fileSize - offset);
                    data = FileStreamHandler.Read(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = FileStreamHandler.Read(path, offset, StreamCommunication.MaxPacketSize);
                    offset += StreamCommunication.MaxPacketSize;
                }

                _streamHandler.Write(data);
                currentPart++;
            }
        }

        public async Task ReceiveFileWithStreams(long fileSize, string fileName)
        {
            long fileParts = StreamCommunication.CalculateFileParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == fileParts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = await _streamHandler.ReadAsync(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = await _streamHandler.ReadAsync(StreamCommunication.MaxPacketSize);
                    offset += StreamCommunication.MaxPacketSize;
                }
                
                FileStreamHandler.Write(fileName, data);
                currentPart++;
            }
        }
    }
}