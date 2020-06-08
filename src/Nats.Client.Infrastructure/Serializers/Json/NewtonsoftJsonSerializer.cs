using Newtonsoft.Json;

namespace Infrastructure.Serializers.Json
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new NonPublicPropertiesResolver(),
            TypeNameHandling = TypeNameHandling.All
        };

        string IJsonSerializer.Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        T IJsonSerializer.Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str, _settings);
        }
    }
}