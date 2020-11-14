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
        public const int IntegerSize = 4;
        public const int LongSize = 8;
        
        public enum Commands{
            RESPONSE,
            REQUEST
        }
        
        public enum RequestCommands
        {
            LOGIN,
            USER_CREATE,
            USER_DELETE,
            USER_MODIFIY,
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
