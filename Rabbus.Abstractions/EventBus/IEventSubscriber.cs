using System;

namespace Rabbus.Abstractions.EventBus
{
    public interface IEventSubscriber : IDisposable
    {
        void Subscribe<T>(T command) where T : IIntegrationEvent;
    }
}