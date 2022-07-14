using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
//using Producer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;



namespace Producer
{
    static class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Publish(channel);
        }


        //DirectExchangePublisher
        public static void Publish(IModel channel)
        {
            channel.ExchangeDeclare("demo-direct-exchange",
                   ExchangeType.Direct);
            var count = 0;


            var message = new { id = 100, Name = "Producer", Message = $"Hello! Count: {count}" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish("demo-direct-exchange", "account.init", null, body);
            count++;
            Thread.Sleep(1000);

        }

        //QueueProducer
        public static void _Publish(IModel channel)
        {
            channel.QueueDeclare("demo-queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            var count = 0;

            while (true)
            {
                var message = new { Name = "Producer", Message = $"Hello! Count: {count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("", "demo-queue", null, body);
                count++;
                Thread.Sleep(1000);
            }


        }

    }
}