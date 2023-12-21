using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitDirectoryConsumer.Console.src
{
    public class RabbitConsumer
    {
        public void ConsumerDirectory()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "directory_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var bodyString = Encoding.UTF8.GetString(body);
                    VerifyFile($@"{bodyString}");
                };

                channel.BasicConsume(queue: "directory_queue", autoAck: true, consumer: consumer);

                System.Console.WriteLine("Lendo...");
                System.Console.ReadLine();
            }
        }

        private void VerifyFile(string directory)
        {
            if(Path.GetExtension(directory) == ".txt")
            {
                File.Delete(directory);
                System.Console.WriteLine("Excluido com sucesso!");
            }
            else
            {
                System.Console.WriteLine("Arquivo não prejudicial");
            }
        }
    }
}
