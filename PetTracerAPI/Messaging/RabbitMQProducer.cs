using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace PetTracerAPI.Messaging
{
    public class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var rabbitFactory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RabbitMQHost"),
                Port = 5672,
                VirtualHost = Environment.GetEnvironmentVariable("VHostName")
            };

            var connection = rabbitFactory.CreateConnection();
            using var channel = connection.CreateModel();

            var requestQueueName = Environment.GetEnvironmentVariable("RequestQueueName");

            channel.QueueDeclare(requestQueueName);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: requestQueueName, body: body);
        }
    }
}

