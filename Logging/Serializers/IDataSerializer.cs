using System.IO;

namespace ig.log4net.Logging.Serializers
{
    public interface IDataSerializer<TObject>
    {
        byte[] Serialize(TObject obj);
    }

    public abstract class BaseDataSerializer<TObject> : IDataSerializer<TObject>
    {
        public abstract byte[] Serialize(TObject obj);

        public static byte[] ReadFully(MemoryStream input)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                input.Write(buffer, 0, read);
            }
            return input.ToArray();
        }
    }
}
