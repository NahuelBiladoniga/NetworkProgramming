using System;
using System.Configuration;
using System.IO;

namespace TCPComm.Protocol
{
    public class CustomFileStream
    {
        public byte[] Read(string path, long offset, int length)
        {
            var data = new byte[length];

            using var fileStream = new FileStream(path, FileMode.Open)
            {
                Position = offset
            };
            var bytesRead = 0;
            while (bytesRead < length)
            {
                var read = fileStream.Read(data, bytesRead, length - bytesRead);
                if (read == 0)
                {
                    throw new Exception("Couldn't not read file");
                }
                bytesRead += read;
            }

            return data;
        }

        public void Write(string relativePath, string fileName, byte[] data)
        {
            var path = GetFullPath();
            var fullPath = Path.Combine(path, relativePath);
            Directory.CreateDirectory(fullPath);

            using var fileStream = new FileStream(fullPath + "\\" + fileName, FileMode.Append);
            fileStream.Write(data, 0, data.Length);
        }

        public FileInfo[] GetFileInfoInDirectory(string relativePath)
        {
            var path = GetFullPath();
            var fullPath = Path.Combine(path, relativePath);

            var directory = new DirectoryInfo(fullPath);
            var files = directory.GetFiles();

            return files;
        }

        private string GetFullPath() => ConfigurationManager.AppSettings["SaveFilePath"] != null ? ConfigurationManager.AppSettings["SaveFilePath"].ToString() : string.Empty;
    }
}
