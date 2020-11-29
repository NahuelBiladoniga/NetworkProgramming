using System;
using System.Collections.Generic;
using System.Text;

namespace TCPComm.Protocol
{
    public static class ProtocolConstants
    {
        public const int COMMAND_SIZE = 3;
        public const int REQUEST_SIZE = 4;
        public const int MESSAGE_SIZE = 20;
        public const int USER_NAME = 20;
        public const int USER_EMAIL = 20;
        public const int LAST_CONNECTION = 20;
        public const int PHOTO_ID = 20;
        public const int PHOTO_NAME = 20;
        public const int PHOTO_LENGTH = 8;
        public const int MAX_PACKET_SIZE = 32768;
        public const int FIXED_FILE_NAME_LENGTH = 4;
        public const int SIZE = 4;


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
