using System;
using System.Text;

namespace TCPComm.Protocol
{
    public static class ConversionHandler
    {
        public static byte[] ConvertStringToBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string ConvertBytesToString(byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        public static byte[] ConvertIntToBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public static int ConvertBytesToInt(byte[] value)
        {
            return BitConverter.ToInt32(value);
        }

        public static byte[] ConvertLongToBytes(long value)
        {
            return BitConverter.GetBytes(value);
        }

        public static long ConvertBytesToLong(byte[] value)
        {
            return BitConverter.ToInt64(value);
        }
    }
}