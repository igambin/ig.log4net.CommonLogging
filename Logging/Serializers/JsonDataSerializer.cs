using System.IO;
using Newtonsoft.Json;

namespace ig.log4net.Logging.Serializers
{
    public class JsonDataSerializer<TObject> : BaseDataSerializer<TObject>
    {
        public override byte[] Serialize(TObject obj)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            using (var writer = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(writer, obj);
                writer.Flush();
                return ReadFully(stream);
            }
        }
    }
}