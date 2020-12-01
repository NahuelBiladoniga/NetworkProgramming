using System;
using System.Collections.Generic;
using System.Text;

namespace TCPComm.Protocol
{
    public static class ProtocolConstants
    {
        public const int CommandSize = 3;
        public const int RequestSize = 4;
        public const int MessageSize = 20;
        public const int UserName = 20;
        public const int UserEmail = 20;
        public const int LastConnection = 20;
        public const int PhotoId = 20;
        public const int PhotoName = 20;
        public const int PhotoLength = 8;
        public const int MaxPacketSize = 32768;
        public const int FixedFileNameLength = 4;
        public const int Size = 4;


        public enum Commands{
            RESPONSE,
            REQUEST
        }

        public enum RequestCommands
        {
            LOGIN,
            USER_CREATE,
            USER_DELETE,
            PHOTO_GET,
            PHOTO_LOAD,
            COMMENT_PHOTO,
            PHOTO_COMMENTS
        }

        public enum ResponseCommands
        {
            OK,
            ERROR,
            LIST_USERS,
            LIST_PHOTOS,
            LIST_COMMENTS
        }
    }
}
