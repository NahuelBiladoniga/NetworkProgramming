using System;
using System.IO;

namespace TCPComm.Protocol
{
    public static class FileHandler
    {
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
        
        public static void WriteFile(string fileName, byte[] data)
        {
            if (FileExists(fileName))
            {
                throw new Exception("File does not exist");
            }
            else
            {
                File.WriteAllBytes(fileName, data);
            }
        }
        
        public static byte[] ReadFile(string path)
        {
            if (FileExists(path))
            {
                return File.ReadAllBytes(path);
            }

            throw new Exception("File does not exist");
        }
        
        public static long GetFileSize(string path)
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Length;
            }

            throw new Exception("File does not exist");
        }
    }
}