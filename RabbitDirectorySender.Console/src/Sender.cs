using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitDirectorySender.Console.src
{
    public class Sender
    {
        public void SenderToRabbit(string directory)
        {
            var connectionFactory = new ConnectionFactory();
            using(var connection = connectionFactory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "directory_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var body = Encoding.UTF8.GetBytes(directory);
                channel.BasicPublish(exchange: "", routingKey: "directory_queue", body: body);
            }

        }
    }
}
