using Nats.Client.Infrastructure.Base;
using Nats.Client.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nats.Client.Infrastructure.Database.Repositories
{
    public interface IMessageForSaveRepository : IRepositoryBase<MessageForSave>
    {
    }
}
