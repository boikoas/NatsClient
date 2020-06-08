using NATS.Client;

namespace Nats.Client.Infrastructure.Messaging.Nats
{
    public sealed class NatsMessage<T>
    {
        public string Subject { get; }
        public string Reply { get; }
        public byte[] Data { get; }
        public T DataObject { get; }
        public ISubscription ArrivalSubscription { get; }

        public NatsMessage(
            string subject, 
            string reply, 
            byte[] data, 
            T dataObject, 
            ISubscription arrivalSubscription)
        {
            Subject = subject;
            Reply = reply;
            Data = data;
            DataObject = dataObject;
            ArrivalSubscription = arrivalSubscription;
        }
    }
}