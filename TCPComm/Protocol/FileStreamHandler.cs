using System;
using System.IO;

namespace TCPComm.Protocol
{
    public static class FileStreamHandler
    {
        public static byte[] Read(string path, long offset, int length)
        {
            if (FileHandler.FileExists(path))
            {
                var data = new byte[length];

                using var fs = new FileStream(path, FileMode.Open) { Position = offset };
                var bytesRead = 0;
                while (bytesRead < length)
                {
                    var read = fs.Read(data, bytesRead, length - bytesRead);
                    if (read == 0)
                        throw new Exception("Error reading file");
                    bytesRead += read;
                }

                return data;
            }

            throw new Exception("File does not exist");
        }

        public static void Write(string fileName, byte[] data)
        {
            if (FileHandler.FileExists(fileName))
            {
                using var fs = new FileStream(fileName, FileMode.Append);
                fs.Write(data, 0, data.Length);
            }
            else
            {
                using var fs = new FileStream(fileName, FileMode.Create);
                fs.Write(data, 0, data.Length);
            }
        }
    }
}