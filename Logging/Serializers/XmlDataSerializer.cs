using System.IO;
using System.Xml.Serialization;

namespace ig.log4net.Logging.Serializers
{
    public class XmlDataSerializer<TObject> : BaseDataSerializer<TObject>
    {
        public override byte[] Serialize(TObject obj)
        {
            using (var stream = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(typeof(TObject));
                xmlSerializer.Serialize(stream, obj);
                return ReadFully(stream);
            }
        }
    }
}
