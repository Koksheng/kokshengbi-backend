namespace kokshengbi.Application.Common.Interfaces.Messaging
{
    public interface IBiMessageProducer
    {
        void SendMessage(string message);
    }
}
