namespace kokshengbi.Infrastructure.Messaging
{
    public interface IBiMessageConsumer
    {
        Task ConsumeMessage(string message, ulong deliveryTag);
        void StartConsuming();
    }
}
