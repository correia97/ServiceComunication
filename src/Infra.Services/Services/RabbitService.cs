﻿using Infra.Service.Interfaces;
using Infra.Services.Interfaces;
using Infra.Services.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Infra.Service.Services
{
    public class RabbitService : IQueueService
    {
        private string QueueRead { get; set; }
        private string QueuePublish { get; set; }
        private ConnectionFactory Factory { get; set; }
        private readonly IMessageHandler MessageHandler;
        public RabbitService(IConfiguration configuration, IMessageHandler handler)
        {
            var conf = configuration.GetSection("rabbit");
            var rabbitMQEndpointHostname = conf["host"].Trim();
            var rabbitMQEndpointAMQPPort = int.Parse(conf["port"].Trim());
            var dockerContainerUsername = conf["user"].Trim();
            var dockerContainerPassword = conf["password"].Trim();
            var dockerContainerVirtualHost = conf["virtualhost"].Trim();
            QueueRead = conf["read"].Trim();
            QueuePublish = conf["publish"].Trim();
            Factory = new ConnectionFactory
            {
                HostName = rabbitMQEndpointHostname,
                Port = rabbitMQEndpointAMQPPort,
                UserName = dockerContainerUsername,
                Password = dockerContainerPassword,
                VirtualHost = dockerContainerVirtualHost,
            };
            MessageHandler = handler;
        }
        public T GetMessage<T>() where T : class
        {
            T message = null;
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var txtMessage = Encoding.UTF8.GetString(body.ToArray());
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    message = JsonConvert.DeserializeObject<T>(txtMessage);
                };

                channel.BasicConsume(queue: QueueRead,
                                     autoAck: false,
                                     consumer: consumer);
            }
            return message;
        }

        public void PublishMessage<T>(T message) where T : class
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                CreateQueue(channel, QueuePublish);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchange: "",
                                     routingKey: QueuePublish,
                                     basicProperties: null,
                                     body: body);
            }
        }

        private static void CreateQueue(IModel channel, string queueName)
        {
            try
            {
                channel.QueueDeclare(queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public static void CreateQueueReader(IConfiguration configuration, IMessageHandler handler)
        {

            Thread.Sleep(TimeSpan.FromMinutes(2));
            var retryOnStartupPolicy = Policy
                                       .Handle<Exception>()
                                       .WaitAndRetry(9, retryAttempt =>
                                            TimeSpan.FromSeconds(Math.Pow(10, retryAttempt)));

            var conf = configuration.GetSection("rabbit");
            var rabbitMQEndpointHostname = conf["host"].Trim();
            var rabbitMQEndpointAMQPPort = int.Parse(conf["port"].Trim());
            var dockerContainerUsername = conf["user"].Trim();
            var dockerContainerPassword = conf["password"].Trim();
            var dockerContainerVirtualHost = conf["virtualhost"].Trim();
            var queueRead = conf["read"].Trim();

            var factory = new ConnectionFactory
            {
                HostName = rabbitMQEndpointHostname,
                Port = rabbitMQEndpointAMQPPort,
                UserName = dockerContainerUsername,
                Password = dockerContainerPassword,
                VirtualHost = dockerContainerVirtualHost,
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            retryOnStartupPolicy.Execute(() =>
            {
                CreateQueue(channel, queueRead);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var txtMessage = Encoding.UTF8.GetString(body.ToArray());
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    handler.Handler(JsonConvert.DeserializeObject<QueueMessage>(txtMessage));

                };

                channel.BasicConsume(queue: queueRead,
                                     autoAck: false,
                                     consumer: consumer);
            });
        }
    }
}
