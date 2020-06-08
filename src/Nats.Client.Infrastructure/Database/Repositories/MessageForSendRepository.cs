using Nats.Client.Domain.Base;
using Nats.Client.Domain.Model;
using Nats.Client.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nats.Client.Infrastructure.Database.Repositories
{
    public class MessageForSendRepository : RepositoryBase<MessageForSend>, IMessageForSendRepository
    {
        public MessageForSendRepository(AppDbContext repositoryContext)
          : base(repositoryContext)
        {
        }
    }
}
