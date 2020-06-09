using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nats.Client.Infrastructure.Serializers.Binary
{
    public sealed class BinaryFormatterSerializer : IBinarySerializer
    {
        private static readonly BinaryFormatter BinaryFormatter = new BinaryFormatter();

        byte[] IBinarySerializer.Serialize<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException($"Object to serialize cannot be null");

            using var memoryStream = new MemoryStream();
            BinaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }

        T IBinarySerializer.Deserialize<T>(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                throw new ArgumentNullException($"Byte array cannot be null or empty");

            using var memoryStream = new MemoryStream(byteArray);
            return (T)BinaryFormatter.Deserialize(memoryStream);
        }
    }
}