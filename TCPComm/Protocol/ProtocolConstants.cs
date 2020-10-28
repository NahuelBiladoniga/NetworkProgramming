using System;
using System.Collections.Generic;
using System.Text;

namespace TCPComm.Protocol
{
    public static class ProtocolConstants
    {
        public const int FixedDataSize = 4;

        public enum COMMANDS{
            RESPONSE,
            REQUEST
        }

        public enum REQUEST_COMMANDS
        {
            LOGIN,
            USER_CREATE,
            USER_DELETE,
            PHOTO_GET,
            PHOTO_LOAD,
            COMMENT_PHOTO,
            PHOTO_COMMENTS
        }

        public enum RESPONSE_COMMANDS
        {
            OK,
            ERROR,
            LIST_USERS,
            LIST_PHOTOS,
            LIST_COMMENTS
        }
    }
}
