using ig.log4net.Extensions;

namespace ig.log4net.Logging.Serializers
{
    public class TextDataSerializer<TObject> : IDataSerializer<TObject>
    {
        public byte[] Serialize(TObject obj)
        {
            var txt = obj as string;
            if (txt == null)
            {
                // if its not a string try to see if it implements ToString() 
                txt = obj.ToString();
                if (txt == obj.GetType().FullName) // if it does not implement ToString() use JsonSerializer and render that
                {
                    var serializer = new JsonDataSerializer<TObject>();
                    return serializer.Serialize(obj);
                }
            }
            return txt.ToByteArray();
        }
    }
}