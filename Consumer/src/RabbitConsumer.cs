using Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.src
{
    public class RabbitConsumer
    {

        public async void ConsumeMessage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "minhaFila",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);


                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var mensagem = JsonConvert.DeserializeObject<UserModel>(Encoding.UTF8.GetString(body));
                    Console.WriteLine($" [Consumidor] Recebida a mensagem, inserindo um dominio de email: '{mensagem.Email}@gmail.com'");
                };
                channel.BasicConsume(queue: "minhaFila",
                                autoAck: true,
                                consumer: consumer);
                Console.WriteLine("Processamento de dado iniciado\n");

                Console.WriteLine("[Exit] Pressione enter para sair");
                Console.ReadLine();
            }
        }
    }
}
