using TCPComm.Protocol;

namespace TCPComm.Dto
{
    public class MessageResponse
    {
        public ProtocolConstants.ResponseCommands responseCommands { get; set; }
        public string Message { get; set; }
    }
}