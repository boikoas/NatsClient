namespace Infrastructure.Serializers.Json
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string str);
    }
}