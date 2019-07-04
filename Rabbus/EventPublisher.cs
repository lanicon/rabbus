using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Rabbus.Abstractions;
using Rabbus.Abstractions.EventBus;

namespace Rabbus
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventPublisher(MessagingOptions options)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(options.ConnectionString)
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public async Task PublishAsync<T>(T command) where T : IIntegrationEvent
        {
            await Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(value: command, formatting: Formatting.Indented,
                    settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                var bytes = Encoding.UTF8.GetBytes(s: json);

                _channel.QueueDeclare(queue: command.Key,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _channel.BasicPublish(exchange: "",
                    routingKey: command.Key,
                    basicProperties: properties,
                    body: bytes);
            });
        }
    }
}
