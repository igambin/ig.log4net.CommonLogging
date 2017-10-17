using System;
using System.Linq;
using System.Reflection;

namespace ig.log4net.Logging.Serializers
{
    class SerializerTypeInfo : Attribute
    {
        internal SerializerTypeInfo(int id, string fileEnding, Type dataSerializerType /* ,...additional params... */)
        {
            this.Id = id;
            this.FileEnding = fileEnding;
            this.DataSerializerType = dataSerializerType;
        }

        public int Id { get; set; }
        public string FileEnding { get; set; }
        public Type DataSerializerType { get; set; }
    }

    public enum SerializerType
    {
        [SerializerTypeInfo(0, "txt" , null)                        ] None,
        [SerializerTypeInfo(1, "xml" , typeof(XmlDataSerializer<>)) ] Xml,
        [SerializerTypeInfo(2, "json", typeof(JsonDataSerializer<>))] Json,
        [SerializerTypeInfo(3, "txt" , typeof(TextDataSerializer<>))] Text
    }

    public static class SerializerTypeHelper
    {
        #region Enum-Finders
        public static SerializerType GetByValue(int x)
        {
            return (SerializerType)x;
        }

        public static SerializerType GetByCode(int x)
        {
            return Enum.GetValues(typeof(SerializerType)).Cast<SerializerType>().FirstOrDefault(a => a.Id() == x);
        }

        public static SerializerType GetByName(string x)
        {
            return Enum.GetValues(typeof(SerializerType)).Cast<SerializerType>().FirstOrDefault(a => a.ToString().Equals(x));
        }
        #endregion

        #region EnumInfo-Property-Accessors
        public static int Value(this SerializerType x)
        {
            return (int)x;
        }

        public static int Id(this SerializerType x)
        {
            return GetAttr(x).Id;
        }

        public static string Name(this SerializerType x)
        {
            return x.ToString();
        }
        public static string FileEnding(this SerializerType x)
        {
            return GetAttr(x).FileEnding;
        }

        public static IDataSerializer<TItem> GetSerializer<TItem>(this SerializerType x)
        {
            if (x == SerializerType.None)
            {
                throw new TypeInitializationException(
                    "The SerializerType 'None' can not be used for serialization.", null);
            }

            var t = GetAttr(x).DataSerializerType;
            var tg = t.MakeGenericType(typeof(TItem));
            try
            {
                ConstructorInfo ci = tg.GetConstructor(Type.EmptyTypes);
                var serializer = (IDataSerializer<TItem>)ci.Invoke(null);
                return serializer;
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(
                    $"Creating a Constructor for {t.Name}<{typeof(TItem).Name}> failed.", ex);
            }
        }

        // additional property accessors ...
        #endregion

        #region custom-enum-helpers
        private static SerializerTypeInfo GetAttr(SerializerType x)
        {
            return (SerializerTypeInfo)Attribute.GetCustomAttribute(ForValue(x), typeof(SerializerTypeInfo));
        }

        private static MemberInfo ForValue(SerializerType x)
        {
            return typeof(SerializerType).GetField(Enum.GetName(typeof(SerializerType), x));
        }
        #endregion
    }

}
