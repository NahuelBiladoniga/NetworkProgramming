using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Domain;
using TCPComm;
using TCPComm.Protocol;

namespace AdminClient
{
  public class FileClient
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine("Starting client...");
      var clientIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0);
      TcpClient client = new TcpClient(clientIpEndPoint);
      Console.WriteLine("Attempting connection to server...");
      await client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 6000);
      NetworkStream stream = client.GetStream();
      var communication = new NetworkCommunication(stream);

      while (true)
      {
        var writeTask = Task.Run(async () => await WriteAsync(communication));
      }
    }

    static async Task WriteAsync(NetworkCommunication communication)
    {
      var data = ParseWrite(Console.ReadLine());
      await communication.WriteAsync(data);
      await ReadAsync(communication);
    }

    static byte[] ParseWrite(string commandConsole)
    {
      var splitedCommand = commandConsole.Split(',');
    byte[] result = new byte[0];
            result = Utils.AddCommandResult(result, "REQ");

            result = Utils.AddCommandResult(result, splitedCommand[0]);

      switch (int.Parse(splitedCommand[0]))
      {
        case (int)ProtocolConstants.RequestCommands.USER_CREATE:
          result = Utils.AddLengthResult(result, User.EmailSize+User.NameSize);
          result = Utils.MergeArrays(result,Utils.FillToSize(splitedCommand[1], User.EmailSize));
          var userDataCreate = Utils.MergeArrays(Utils.FillToSize(splitedCommand[1], User.NameSize),
            Utils.FillToSize(splitedCommand[2], User.NameSize));
          result = Utils.MergeArrays(result,userDataCreate);
          break;
        case (int)ProtocolConstants.RequestCommands.USER_DELETE:
          result = Utils.AddLengthResult(result, User.EmailSize);
          result = Utils.MergeArrays(result,Utils.FillToSize(splitedCommand[1], User.EmailSize));
          break;
        case (int)ProtocolConstants.RequestCommands.USER_MODIFIY:
          result = Utils.AddLengthResult(result, User.EmailSize+User.NameSize);
          result = Utils.MergeArrays(result,Utils.FillToSize(splitedCommand[1], User.EmailSize));
          var userDataModify = Utils.MergeArrays(Utils.FillToSize(splitedCommand[1], User.NameSize),
            Utils.FillToSize(splitedCommand[2], User.NameSize));
          result = Utils.MergeArrays(result,userDataModify);
          break;
      }

      return result;
    }
        
    static async Task ReadAsync(NetworkCommunication communication)
    {
      await communication.ReadAsync(ProtocolConstants.CommandSize);
      var responseCode = BitConverter.ToInt32(await communication.ReadAsync(ProtocolConstants.RequestSize));
      var msg = "";
      var dataLength = Utils.ByteToInt(await communication.ReadAsync(ProtocolConstants.CommandSize));

      switch (responseCode)
      {
        case (int)ProtocolConstants.ResponseCommands.OK:
          msg = Utils.ByteToString(await communication.ReadAsync(ProtocolConstants.MessageSize));
          break;
        case (int)ProtocolConstants.ResponseCommands.ERROR:
          msg = Utils.ByteToString(await communication.ReadAsync(ProtocolConstants.MessageSize));
          break;
      }

      Console.WriteLine(msg);
    }
  }
}