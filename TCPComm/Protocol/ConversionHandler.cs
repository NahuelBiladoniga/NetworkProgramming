using System;
using System.Text;

namespace TCPComm.Protocol
{
    public static class ConversionHandler
    {
        public static byte[] ConvertCommandToByte(ProtocolConstants.Commands command)
        {
            return ConvertShortToBytes((short)command);
        }

        public static byte[] ConvertResponseCommandToByte(ProtocolConstants.ResponseCommands command)
        {
            return ConvertShortToBytes((short)command);
        }

        public static byte[] ConvertRequestCommandToByte(ProtocolConstants.RequestCommands command)
        {
            return ConvertShortToBytes((short)command);
        }

        public static ProtocolConstants.Commands ConvertByteToCommand(byte[] data)
        {
            return (ProtocolConstants.Commands)ConvertBytesToShort(data);
        }

        public static ProtocolConstants.ResponseCommands ConvertByteToResponseCommand(byte[] data)
        {
            return (ProtocolConstants.ResponseCommands)ConvertBytesToShort(data);
        }

        public static ProtocolConstants.ResponseCommands ConvertByteToRequestCommand(byte[] data)
        {
            return (ProtocolConstants.ResponseCommands)ConvertBytesToShort(data);
        }

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

        public static byte[] ConvertShortToBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        public static short ConvertBytesToShort(byte[] value)
        {
            return BitConverter.ToInt16(value);
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