using System;
using System.Collections.Generic;
using System.Text;

namespace TCPComm.Protocol
{
    public static class ProtocolConstants
    {
        public const int ShortTypeLength = 2;
        public const int IntegerTypeLength = 4;
        public const int LongTypeLength = 8;
        public const int DateTimeTypeLength = 8;
        public const int ResponseMessageLength = 40;
        
        public const int MaxPacketSize = 32768;

        public enum Commands{
            RESPONSE,
            REQUEST
        }

        public enum RequestCommands
        {
            LOGIN,
            CREATE_USER,
            DELETE_USER,
            VIEW_PHOTOS,
            VIEW_USERS,
            UPLOAD_PHOTO,
            COMMENT_PHOTO,
            VIEW_COMMENTS
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
