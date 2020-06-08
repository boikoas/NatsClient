namespace Nats.Client.Domain
{
    public static class Const
    {
        public static class Nats
        {
            public const string Queue = "nats-queue";
        }
        
        public static class EventStore
        {
            public const string SubscriptionGroup = "evtstore-group";
            public const string EventTypePrefix = "$et-";
        }
        
        public static class Message
        {
            public static readonly string  InternalServerError;
            public static readonly string  ItemNotFound,ItemWasDeleted,ItemWasCreated;
        }
    }
}