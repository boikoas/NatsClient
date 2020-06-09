using Nats.Client.Domain.Base;
using Nats.Client.Domain.Model;

namespace Nats.Client.Infrastructure.Database.Repositories
{
    public class MessageForSaveRepository : RepositoryBase<MessageForSave>, IMessageForSaveRepository
    {
        public MessageForSaveRepository(AppDbContext repositoryContext)
          : base(repositoryContext)
        {
        }
    }
}