using MessageForSave = Nats.Client.Domain.Model.MessageForSave;
using Nats.Client.Infrastructure.Messaging.Nats;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Services
{
    public interface IMessageProcessingService
    {
        Task MessageProcessing(MessageForSave message);
    }
}
