using MediatR;

namespace Rabbus.Abstractions.EventBus
{
    public interface IIntegrationEvent : IRequest
    {
        string Key { get; }
    }
}