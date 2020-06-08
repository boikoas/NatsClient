using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nats.Client.Infrastructure.Serializers.Binary;
using NATS.Client;

namespace Nats.Client.Infrastructure.Messaging.Nats
{
    public interface INatsManager : IDisposable
    {
        Task PublishAsync<T>(string topic, T data); 
        Task<ReplyMessage> RequestAsync<TRequest>(string topic, TRequest data, int timeout = 30);
        IAsyncSubscription SubscribeAsync<T>(string topic, Action<NatsMessage<T>> action);
        IAsyncSubscription SubscribeAsync<T>(string topic, string queue, Action<NatsMessage<T>> action);
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
                opts.ReconnectedEventHandler += ReconnectedEventHandler;
                opts.ClosedEventHandler += ClosedEventHandler;
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

        IAsyncSubscription INatsManager.SubscribeAsync<T>(string topic, Action<NatsMessage<T>> action)
        {
            return SubscribeAsync(topic, string.Empty, action);
        }
        
        IAsyncSubscription INatsManager.SubscribeAsync<T>(string topic, string queueName, Action<NatsMessage<T>> action)
        {
            return SubscribeAsync(topic, queueName, action);
        }

        private IAsyncSubscription SubscribeAsync<T>(string topic, string queueName, Action<NatsMessage<T>> action) 
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
        
        private EventHandler<MsgHandlerEventArgs> EventHandlerFactory<T>(Action<NatsMessage<T>> action) 
        {
            return (sender, args) =>
            {
                var natsMessage = NatMessageFactory<T>(args);
                action?.Invoke(natsMessage); 
            };
        }
        
        private NatsMessage<T> NatMessageFactory<T>(MsgHandlerEventArgs args) 
        {
            var message = _binarySerializer.Deserialize<T>(args.Message.Data);
            return new NatsMessage<T>(
                args.Message.Subject,
                args.Message.Reply,
                args.Message.Data,
                message,
                args.Message.ArrivalSubcription
            );
        }
        
        private void ReconnectedEventHandler(object sender, ConnEventArgs e)
        {
            throw new Exception($"Reconnected to NATS.");
        }
        
        private void ClosedEventHandler(object sender, ConnEventArgs e)
        {
            throw new Exception($"NATS connection closed.");
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