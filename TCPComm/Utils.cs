using System;
using System.Linq;
using System.Text;
using TCPComm.Protocol;

namespace TCPComm
{
    public static class Utils
    {
        public static byte[] FillToSize(string v, int size)
        {
            var data = Encoding.UTF8.GetBytes(v);
            
            if (data.Length < size)
            {
                var fill = new byte[size - data.Length];
                for (int i = 0; i < fill.Length; i++)
                {
                    fill[i] = 0x0;
                }
                data = fill.Concat(data).ToArray();
            }
            
            return data;
        }
        
        public static byte[] GetFileFromFs(string v)
        {
            throw new NotImplementedException();
        }

        public static byte[] MergeArrays(byte[] arr1,byte[]arr2)
        {
            return arr1.Concat(arr2).ToArray();
        }

        public static string ByteToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static DateTime ByteToDateTime(byte[] data)
        {
            return Convert.ToDateTime(data);
        }

        public static int ByteToInt(byte[] data)
        {
            return BitConverter.ToInt32(data);
        }
        
        public static byte[] AddCommandResult(byte[] result, string cmd)
        {
            return MergeArrays(result,FillToSize(cmd, ProtocolConstants.CommandSize));
        }

        public static byte[] AddLengthResult(byte[] result, int size)
        {
            return MergeArrays(result,FillToSize(size.ToString(), ProtocolConstants.IntegerSize));
        }

    }
}