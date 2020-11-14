using TCPComm.Protocol;

namespace AdminServer
{
    public class ResponseDTO
    {
        public string Message { get; set; }
        public ProtocolConstants.ResponseCommands Code { get; set; }
    }
}