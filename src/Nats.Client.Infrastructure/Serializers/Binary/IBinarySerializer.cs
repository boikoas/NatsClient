namespace Nats.Client.Infrastructure.Serializers.Binary
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(byte[] byteArray);
    }
}