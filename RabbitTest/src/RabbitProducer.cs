using Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.src
{
    public class RabbitProducer
    {
        private UserModel model;

        public RabbitProducer()
        {
            model = new UserModel() { Name = "José", Address = "Rua teste", Email = "joseadauto923" };
        }
        public void CreateProducer()
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

                while (true)
                {
                    string mensagem = JsonConvert.SerializeObject(model);

                    var body = Encoding.UTF8.GetBytes(mensagem);
                    channel.BasicPublish(exchange: "",
                                         routingKey: "minhaFila",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($" [Produtor] Enviada a mensagem: '{mensagem}'");

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
