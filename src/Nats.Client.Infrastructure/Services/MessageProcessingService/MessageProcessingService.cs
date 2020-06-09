using Nats.Client.Domain.Model;
using Nats.Client.Infrastructure.Database.Repositories;
using Nats.Client.Infrastructure.Dispatchers;
using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Services
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMessageForSaveRepository _messageForSaveRepository;

        public MessageProcessingService(ICommandDispatcher commandDispatcher,
                                        IMessageForSaveRepository messageForSaveRepository)
        {
            _commandDispatcher = commandDispatcher;
            _messageForSaveRepository = messageForSaveRepository;
        }

        public async Task MessageProcessing(MessageForSave message)
        {
            await _messageForSaveRepository.CreateAsync(message);
        }
    }
}