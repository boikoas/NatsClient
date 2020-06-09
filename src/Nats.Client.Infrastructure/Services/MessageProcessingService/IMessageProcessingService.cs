using System.Threading.Tasks;
using MessageForSave = Nats.Client.Domain.Model.MessageForSave;

namespace Nats.Client.Infrastructure.Services
{
    public interface IMessageProcessingService
    {
        Task MessageProcessing(MessageForSave message);
    }
}