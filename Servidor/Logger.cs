using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;

namespace Server
{
    public static class Logger
    {
        static MessageQueue messageQueue;

        public static void StartQueue(string path)
        {
            messageQueue = new MessageQueue(path);
        }

        public static void LogMessage(Log log)
        {
            messageQueue.Send(log);
        }

        public static void CloseQueue()
        {
            messageQueue.Close();
        }
    }
}
