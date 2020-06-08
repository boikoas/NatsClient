using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Services.ActionWriterService
{
    public interface IActionWriterService
    {
        Task BeginConsumeAsync();
    }
}
