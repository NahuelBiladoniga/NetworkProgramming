using System.Configuration;

namespace TCPComm.Constants
{

    public class ServerConstants
    {
        public static int SERVER_PORT = GetPort("SERVER_PORT");
        public const int COMMAND_LENGTH = 4;
        public const char PARSER_CHARACTER = '#';

        public const int FIXED_FILE_NAME_LENGTH = 4;
        public const int FIXED_FILE_SIZE_LENGTH = 8;
        public const int MAX_PACKET_SIZE = 32768; // 32KB

        public const string SERVER_WINDOW_TITLE = "ADMINISTRACIÓN DEL SERVIDOR";

        private static int GetPort(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            int port;
            var isInt = int.TryParse(value, out port);
            return isInt ? port : 0;
        }
    }
}

