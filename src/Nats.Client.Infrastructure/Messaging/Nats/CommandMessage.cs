using System;

namespace Nats.Client.Infrastructure.Messaging.Nats
{
    [Serializable]
    public sealed class CommandMessage<T>
    {
        public Guid Id { get; }
        public T Data { get; }

        public CommandMessage(T data)
        {
            Id = Guid.NewGuid();
            Data = data;
        }
    }
}