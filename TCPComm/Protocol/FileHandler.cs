using System;
using System.IO;
using System.Threading.Tasks;

namespace TCPComm.Protocol
{
    public static class FileHandler
    {
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
        
        public static string FileExtension(string path)
        {
            return new FileInfo(path).Extension;
        }

        public static string FileName(string path)
        {
            return new FileInfo(path).Name;
        }


        public static long GetFileSize(string path)
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Length;
            }

            throw new Exception("File does not exist");
        }
        
        public static void SendFileWithStream(long fileSize, string path, StreamCommunication streamCommunication)
        {
            long fileParts = ProtocolHelpers.CalculateFileParts(fileSize);
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
                    data = FileStreamHandler.Read(path, offset, ProtocolConstants.MaxPacketSize);
                    offset += ProtocolConstants.MaxPacketSize;
                }

                streamCommunication.Write(data);
                currentPart++;
            }
        }

        public static async Task ReceiveFileWithStreams(long fileSize, string fileName, StreamCommunication streamCommunication)
        {
            long fileParts = ProtocolHelpers.CalculateFileParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == fileParts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = await streamCommunication.ReadAsync(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = await streamCommunication.ReadAsync(ProtocolConstants.MaxPacketSize);
                    offset += ProtocolConstants.MaxPacketSize;
                }
                
                FileStreamHandler.Write(fileName, data);
                currentPart++;
            }
        }
    }
}