using System;
using Nats.Setvice.Domain.Base;

namespace Nats.Client.Infrastructure.Messaging.Nats
{
    [Serializable]
    public sealed class ReplyMessage : BaseCommand
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        private ReplyMessage(
            bool isSuccess,
            string message)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        public static ReplyMessage Success(string message = null)
        {
            return new ReplyMessage(true, message);
        }
        
        public static ReplyMessage Failed(string message = null)
        {
            return new ReplyMessage(false, message);
        }
    }
}