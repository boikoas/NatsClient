using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Serializers.Json
{
    public class NonPublicPropertiesResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var prop = base.CreateProperty(member, memberSerialization);
            if (!(member is PropertyInfo pi)) 
                return prop;
            prop.Writable = (pi.SetMethod != null);
            return prop;
        }
    }
}