using Nats.Client.Infrastructure.Dispatchers;
using Nats.Client.Infrastructure.Messaging.Nats;
using System;
using System.Threading;
using System.Threading.Tasks;
using MessageForSave = Nats.Client.Domain.Model.MessageForSave;

namespace Nats.Client.Infrastructure.Services.ActionWriterService
{
    public class ActionWriterService : IActionWriterService
    {
        private readonly IMessageProcessingService _messageProcessingService;
        private readonly ICommandDispatcher _commandDispatcher;

        public ActionWriterService(ICommandDispatcher commandDispatcher, IMessageProcessingService messageProcessingService)
        {
            _messageProcessingService = messageProcessingService;
            _commandDispatcher = commandDispatcher;
        }

        private void NatsClient_GotMessage(NatsMessage<MessageForSave> message)
        {
            var task = _messageProcessingService.MessageProcessing(message.DataObject);
            task.Wait();
        }

        public async Task BeginConsumeAsync()
        {
            try
            {
                _commandDispatcher.SubscribeAsync("NATS", NatsClient_GotMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(5000);
                await BeginConsumeAsync();
            }
        }
    }
}