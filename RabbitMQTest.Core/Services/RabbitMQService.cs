using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTest.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest.Core.Services
{
    public class RabbitMQService:IMessageQueue
    {
        private readonly string _uri;
        private readonly ConnectionFactory _factory;
        private  IConnection _connection;
        private IModel _channel;

        IModel channel => _channel ?? (_channel = GetChannel());

        public IConnection Connection { get
            {
                if (_connection == null || !_connection.IsOpen)
                    _connection = CreateConnection();
                return _connection;
            } }
        public RabbitMQService()
        {
            _uri = "amqps://jnwmichx:CQptElHZJIYh8IuIv1rYlnE3iIHT_okk@rattlesnake.rmq.cloudamqp.com/jnwmichx";
            _factory = new ConnectionFactory() { Uri = new Uri(_uri) };
        }

        public void SendMessageDefaultExchange(string message, string queue)
        {
            byte[] messageToBytes = stringToBytes(message);
            channel.QueueDeclare("message_queue",durable:true,false,false);
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish("",queue,basicProperties:properties,body:messageToBytes);
            Console.WriteLine("Mesaj Başarıyla Gönderildi");
        }
        
        public void SendMessage(string message, string queue, ExchangeTypes exchangeType)
        {
            byte[] messageToBytes= stringToBytes(message);
            channel.ExchangeDeclare("test_exchange2", exchangeType.ToString(), durable: true, autoDelete: false,arguments:null);
            CreateQueue(queue);
            if (exchangeType == ExchangeTypes.direct)
                channel.QueueBind(queue, "test_exchange2", queue);
            else if (exchangeType == ExchangeTypes.fanout)
                channel.QueueBind(queue, "test_exchange2", "");
            else
                channel.QueueBind(queue, "test_exchange2", "test*");

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish("", queue, basicProperties: properties, body: messageToBytes);
            Console.WriteLine("Mesaj Başarıyla Gönderildi");
        }
        public void GetMessageDefaultExchange(string queue)
        {
            CreateQueue(queue);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Mesaj Alındı: {0}", message);
            };
            channel.BasicConsume(queue, true, consumer);
        }
        
        private IModel GetChannel()
        {
            return Connection.CreateModel();
        }
        private IConnection CreateConnection()
        {
            return _factory.CreateConnection();
        }
        private void CreateQueue(string queue)
        {
            channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
        }

        private byte[] stringToBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
    }
}
