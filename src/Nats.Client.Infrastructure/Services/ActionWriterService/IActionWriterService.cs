using System.Threading.Tasks;

namespace Nats.Client.Infrastructure.Services.ActionWriterService
{
    public interface IActionWriterService
    {
        Task BeginConsumeAsync();
    }
}