using System;
using System.Collections.Generic;
using System.Text;

namespace TCPComm.Protocol
{
    public static class ProtocolConstants
    {
        // public const int CommandSize = 3;
        // public const int RequestSize = 4;
        // public const int MessageSize = 20;
        // public const int LastConnection = 20;
        // public const int MaxPacketSize = 32768;
        // public const int FixedFileNameLength = 4;
        // public const int Size = 4;

        public const int RequestTypeLength = 1;
        public const int CommandTypeLength = 1;
        public const int IntegerTypeLength = 4;
        public const int LongTypeLength = 8;

        public enum Commands{
            Response,
            Request
        }

        public enum RequestCommands
        {
            Login,
            Createuser,
            DeleteUser,
            ViewPhotos,
            ViewUsers,
            UploadPhoto,
            CommentPhoto,
            ViewComments
        }

        public enum ResponseCommands
        {
            Ok,
            Error,
            ListUsers,
            ListPhotos,
            ListComments
        }
    }
}
