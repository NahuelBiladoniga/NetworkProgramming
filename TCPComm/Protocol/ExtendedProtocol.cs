using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TCPComm.Protocol
{
    public class ExtendedProtocol : Protocol
    {
        private CustomFileStream _fileStream;

        public ExtendedProtocol(Socket socket, CustomFileStream fileStream) : base(socket)
        {
            _fileStream = fileStream;

        }

        public void SendDataFile(string path)
        {
            long fileSize = GetFileSize(path);
            if (fileSize > 1e+8)
            {
                throw new Exception("The file size must be less than 100 megabytes");
            }

            string fileName = GetFileName(path);
            byte[] header = CreateFileHeader(fileName, fileSize);
            byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);

            SendData(header, header.Length);
            SendData(nameBytes, nameBytes.Length);

            long parts = GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    int lastPartSize = (int)(fileSize - offset);
                    data = _fileStream.Read(path, offset, lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = _fileStream.Read(path, offset, ProtocolConstants.MAX_PACKET_SIZE);
                    offset += ProtocolConstants.MAX_PACKET_SIZE;
                }

                SendData(data, data.Length);
                currentPart++;
            }
        }

        public static long GetParts(long fileSize)
        {
            var parts = fileSize / ProtocolConstants.MAX_PACKET_SIZE;
            return parts * ProtocolConstants.MAX_PACKET_SIZE == fileSize ? parts : parts + 1;
        }

        private long GetFileSize(string path)
        {
            FileExists(path);
            return new FileInfo(path).Length;
        }

        private string GetFileName(string path)
        {
            FileExists(path);
            return new FileInfo(path).Name;
        }

        private bool FileExists(string path) => File.Exists(path) ? true : throw new Exception($"File {path} not exists");

        public void RecieveFile(string relativePath)
        {
            var header = RecieveData(GetLength());
            var fileNameSize = BitConverter.ToInt32(header, 0);
            var fileSize = BitConverter.ToInt64(header, ProtocolConstants.FIXED_FILE_NAME_LENGTH);

            var fileName = Encoding.UTF8.GetString(RecieveData(fileNameSize));

            long parts = GetParts(fileSize);
            long offset = 0;
            long currentPart = 1;

            while (fileSize > offset)
            {
                byte[] data;
                if (currentPart == parts)
                {
                    var lastPartSize = (int)(fileSize - offset);
                    data = RecieveData(lastPartSize);
                    offset += lastPartSize;
                }
                else
                {
                    data = RecieveData(ProtocolConstants.MAX_PACKET_SIZE);
                    offset += ProtocolConstants.MAX_PACKET_SIZE;
                }
                _fileStream.Write(relativePath, fileName, data);
                currentPart++;
            }
        }
        public byte[] CreateFileHeader(string fileName, long fileSize)
        {
            byte[] header = new byte[GetLength()];
            byte[] fileNameData = BitConverter.GetBytes(Encoding.UTF8.GetBytes(fileName).Length);

            if (fileNameData.Length != ProtocolConstants.FIXED_FILE_NAME_LENGTH)
            {
                throw new Exception("There is something wrong with the file name");
            }

            byte[] fileSizeData = BitConverter.GetBytes(fileSize);

            Array.Copy(fileNameData, 0, header, 0, ProtocolConstants.FIXED_FILE_NAME_LENGTH);
            Array.Copy(fileSizeData, 0, header, ProtocolConstants.FIXED_FILE_NAME_LENGTH, ProtocolConstants.FIXED_FILE_NAME_LENGTH);

            return header;
        }

        public static int GetLength() => ProtocolConstants.FIXED_FILE_NAME_LENGTH + ProtocolConstants.FIXED_FILE_NAME_LENGTH;
    }
}

