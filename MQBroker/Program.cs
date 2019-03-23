using System;
using System.Text;
using System.Configuration;
using RabbitMQ.Client;
using NLog;

namespace MQBroker
{
    class Send
    {
        public static void Main()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                
                channel.QueueDeclare(queue: "NetCorePersons", durable: false, exclusive: false, autoDelete: false, arguments: null);
                var n = int.Parse(ConfigurationManager.AppSettings["quantity"]);
                
                for(int i=0;i<n ;i++)
                {

                    Person pers = new Person();   
                    byte[] jsonbyted = Encoding.Unicode.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(pers));
                    channel.BasicPublish(exchange: "", routingKey: "NetCorePersons", basicProperties: null, body: jsonbyted);
                }
                
                Console.WriteLine("Press [Enter] to exit");
                Console.ReadKey();
            }

        }
    }
   
}
