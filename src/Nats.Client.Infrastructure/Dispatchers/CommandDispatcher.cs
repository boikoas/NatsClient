using Nats.Client.Domain.Model;
using Nats.Client.Infrastructure.Messaging.Nats;
using NATS.Client;
using System;
using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Dispatchers
{
    public interface ICommandDispatcher
    {
        IAsyncSubscription SubscribeAsync(string topic, Action<NatsMessage<MessageForSave>> action);

        Task<ReplyMessage> SendAsync<T>(T command);

        Task PublishAsync<T>(string topic, T data);
    }

    public sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly INatsManager _natsManager;

        public CommandDispatcher(INatsManager natsManager)
        {
            _natsManager = natsManager;
        }

        Task<ReplyMessage> ICommandDispatcher.SendAsync<T>(T command)
        {
            return _natsManager.RequestAsync(
                typeof(T).Name,
                new CommandMessage<T>(command));
        }

        public Task PublishAsync<T>(string topic, T data)
        {
            return _natsManager.PublishAsync<T>(topic, data);
        }

        public IAsyncSubscription SubscribeAsync(string topic, Action<NatsMessage<MessageForSave>> action)
        {
            return _natsManager.SubscribeAsync(
                topic,
                action);
        }
    }
}