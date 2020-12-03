using RabbitMQ.Client;
using System.Text;

namespace WebApiAdmin.Services
{
    public class LogService
    {
        private const string QueueName = "QueueForLogs";
        private IModel Channel;

        public LogService()
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            IConnection connection = connectionFactory.CreateConnection();
            Channel = connection.CreateModel();
            QueueDeclare();
        }

        public void SendLogs(string log)
        {
            PublishLog(log);
        }

        private void QueueDeclare()
        {
            Channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void PublishLog(string log)
        {
            if (!string.IsNullOrEmpty(log))
            {
                var body = Encoding.UTF8.GetBytes(log);
                Channel.BasicPublish(exchange: string.Empty, routingKey: QueueName, basicProperties: null, body: body);
            }
        }
    }
}