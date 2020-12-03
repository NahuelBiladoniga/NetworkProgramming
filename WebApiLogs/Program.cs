using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WebApiLogs.Repository;

namespace WebApiLogs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            ReceiveLogs();

            CreateHostBuilder(args).Build().Run();
        }

        private static void ReceiveLogs()
        {
            IModel channel = new ConnectionFactory { HostName = "localhost" }.CreateConnection().CreateModel();

            channel.QueueDeclare("QueueForLogs", false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            new Thread(() =>
            {
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    LogRepository.GetInstance().AddLogEntry(message);
                    Console.WriteLine(message);
                };

                channel.BasicConsume("QueueForLogs", true, consumer);
            }).Start();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:10391");
                });
    }
}
