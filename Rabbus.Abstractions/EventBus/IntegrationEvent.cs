namespace Rabbus.Abstractions.EventBus
{
    public abstract class IntegrationEvent<T> : IIntegrationEvent
    {
        public string Key => GetTypeKey();

        public static string GetTypeKey() => typeof(T).FullName;
    }
}