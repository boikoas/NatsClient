using Nats.Client.Domain.Base;
using Nats.Client.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

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
