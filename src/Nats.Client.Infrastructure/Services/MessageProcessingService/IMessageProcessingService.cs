using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Services
{
    public interface IMessageProcessingService
    {
        Task MessageProcessing();
        
    }
}
