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
            LOGIN= 1,
            USER_CREATE=2,
            USER_DELETE=3,
            PHOTO_GET=4,
            PHOTO_LOAD=5,
            COMMENT_PHOTO=6,
            PHOTO_COMMENTS=7
        }

        public enum RESPONSE_COMMANDS
        {
            OK = 1,
            ERROR = 2,
            LIST_USERS = 3,
            LIST_PHOTOS = 4,
            LIST_COMMENTS = 5
        }
    }
}
