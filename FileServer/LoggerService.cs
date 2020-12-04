using RabbitMQ.Client;
using System.Text;

namespace FileServer
{
    public class LoggerService
    {
        public void SendMessages(string logToSend)
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();
            QueueDeclare(channel);
            PublishMessage(logToSend, channel);
        }

        private void QueueDeclare(IModel channel)
        {
            channel.QueueDeclare(queue: "QueueForLogs", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishMessage(string message, IModel channel)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: string.Empty, routingKey: "QueueForLogs", basicProperties: null, body: body);
            }
        }
    }
}
