using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQReceiver
{
    class Receiver
    {
        
        public static void Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port=5672 };
            Handler.GetCache();
            Handler.logger.Info("Receiver Started");
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "NetCorePersons",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.Unicode.GetString(body);
                        ThreadPool.QueueUserWorkItem(Handler.ProceedRequestAsync,message);
                    };
                    channel.BasicConsume(queue: "NetCorePersons",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine("Receiver is waiting for requests. Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
    }
