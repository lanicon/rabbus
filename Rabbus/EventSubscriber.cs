using System;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Rabbus.Abstractions;
using Rabbus.Abstractions.EventBus;

namespace Rabbus
{
    public class EventSubscriber : IEventSubscriber
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EventSubscriber> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventSubscriber(MessagingOptions options, IMediator mediator, ILogger<EventSubscriber> logger)
        {
            _mediator = mediator;
            _logger = logger;

            var factory = new ConnectionFactory { Uri = new Uri(options.ConnectionString) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        public void Subscribe<T>(T command) where T : IIntegrationEvent
        {
            _channel.QueueDeclare(queue: command.Key,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                var body = args.Body;
                var message = Encoding.UTF8.GetString(body);

                var request = JsonConvert.DeserializeObject<T>(message);

                _logger.LogInformation("Command received: {0}", command.Key);
                _logger.LogInformation("Command parameters for <{0}>: {1}", command.Key, message);

                await _mediator.Send(request);

                _logger.LogInformation("Command <{0}> completed", command.Key);

                _channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: command.Key,
                autoAck: false,
                consumer: consumer);
        }
    }
}
