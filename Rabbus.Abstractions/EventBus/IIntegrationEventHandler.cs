using MediatR;

namespace Rabbus.Abstractions.EventBus
{
    public interface IIntegrationEventHandler<in T> : IRequestHandler<T> where T : IIntegrationEvent
    {
    }
}
