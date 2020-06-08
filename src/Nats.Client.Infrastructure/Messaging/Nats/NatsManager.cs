using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nats.Client.Infrastructure.Serializers.Binary;
using NATS.Client;
using MessageForSave = Nats.Client.Domain.Model.MessageForSave;

namespace Nats.Client.Infrastructure.Messaging.Nats
{
    public interface INatsManager : IDisposable
    {
        Task PublishAsync<T>(string topic, T data); 
        Task<ReplyMessage> RequestAsync<TRequest>(string topic, TRequest data, int timeout = 30);
        IAsyncSubscription SubscribeAsync(string topic, Action<NatsMessage<MessageForSave>> action);
        IAsyncSubscription SubscribeAsync(string topic, string queue, Action<NatsMessage<MessageForSave>> action);
    }
    
    public sealed class NatsManager : INatsManager
    {
        private readonly IBinarySerializer _binarySerializer;
        private Lazy<IConnection> _connectionFactory;
        private readonly INatsConfiguration _natsConfiguration;

        public NatsManager(
            IBinarySerializer binarySerializer,
            INatsConfiguration natsConfiguration)
        {
            _binarySerializer = binarySerializer;
            _natsConfiguration = natsConfiguration;
            CreateConnection();
        }

        private void CreateConnection()
        {
            _connectionFactory = new Lazy<IConnection>(() =>
            {
                var opts = ConnectionFactory.GetDefaultOptions();
                opts.Url = _natsConfiguration.Url;
                opts.AsyncErrorEventHandler += AsyncErrorEventHandler;

                return new ConnectionFactory().CreateConnection(opts);
            });
        }

        Task INatsManager.PublishAsync<T>(string topic, T data)
        {
            return Task.Run(() =>
            {
                var serializeData = _binarySerializer.Serialize(data);
                _connectionFactory.Value?.Publish(topic, serializeData);               
            });
        }

        async Task<ReplyMessage> INatsManager.RequestAsync<TRequest>(string topic, TRequest data, int timeout)
        {
            var serializeData = _binarySerializer.Serialize(data);
            if (_connectionFactory.Value == null) 
                return default;
            var response = await _connectionFactory.Value.RequestAsync(topic, serializeData, (int)TimeSpan.FromSeconds(timeout).TotalMilliseconds);
            return _binarySerializer.Deserialize<ReplyMessage>(response.Data);
        }

        IAsyncSubscription INatsManager.SubscribeAsync(string topic, Action<NatsMessage<MessageForSave>> action)
        {
            return SubscribeAsync(topic, string.Empty, action);
        }
        
        IAsyncSubscription INatsManager.SubscribeAsync(string topic, string queueName, Action<NatsMessage<MessageForSave>> action)
        {
            return SubscribeAsync(topic, queueName, action);
        }

        private IAsyncSubscription SubscribeAsync(string topic, string queueName, Action<NatsMessage<MessageForSave>> action) 
        {
            if (string.IsNullOrEmpty(queueName))
            {
                return _connectionFactory.Value?.SubscribeAsync(
                    topic, 
                    EventHandlerFactory(action)); 
            }
            return _connectionFactory.Value?.SubscribeAsync(
                topic, 
                queueName, 
                EventHandlerFactory(action));
        }
        
        private EventHandler<MsgHandlerEventArgs> EventHandlerFactory(Action<NatsMessage<MessageForSave>> action) 
        {
            return (sender, args) =>
            {
                var natsMessage = NatMessageFactory(args);
                action?.Invoke(natsMessage); 
            };
        }
        
        private NatsMessage<MessageForSave> NatMessageFactory(MsgHandlerEventArgs args) 
        {
            var message = _binarySerializer.Deserialize<MessageForSave>(args.Message.Data);
            return new NatsMessage<MessageForSave>(
                args.Message.Subject,
                args.Message.Reply,
                args.Message.Data,
                message,
                args.Message.ArrivalSubcription
            );
        }
        



        private void AsyncErrorEventHandler(object sender, ErrEventArgs e)
        {
            throw new Exception($"Error occurred. Subject:{e.Subscription.Subject} Error:{e.Error}");
        }
        
        void IDisposable.Dispose()
        {
            if(_connectionFactory.IsValueCreated)
                _connectionFactory.Value?.Dispose();
        }
    }
}