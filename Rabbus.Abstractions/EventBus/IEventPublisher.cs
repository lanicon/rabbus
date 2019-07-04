using System;
using System.Threading.Tasks;

namespace Rabbus.Abstractions.EventBus
{
    public interface IEventPublisher : IDisposable
    {
        Task PublishAsync<T>(T command) where T : IIntegrationEvent;
    }
}
