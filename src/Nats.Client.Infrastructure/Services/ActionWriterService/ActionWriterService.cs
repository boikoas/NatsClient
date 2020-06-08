using Nats.Client.Infrastructure.Dispatchers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Services.ActionWriterService
{
    public class ActionWriterService : IActionWriterService
    {
        private readonly IMessageProcessingService _messageProcessingService;
        public ActionWriterService(IMessageProcessingService messageProcessingService)
        {
            _messageProcessingService = messageProcessingService;
        }

        public async Task BeginConsumeAsync()
        {
            try
            {
                while (true)
                {
                        await _messageProcessingService.MessageProcessing();
                        await Task.Delay(3000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(5000);
                await BeginConsumeAsync();
            }
        }


    }
}
