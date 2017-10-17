using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ig.log4net.Extensions
{
    public static class GenericConversions
    {
        public static T GetTOrDefault<T>(this string input, [Optional] T defaultValue) where T : IConvertible
        {
            T x;
            try
            {
                x = (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception)
            {
                if (defaultValue == null)
                {
                    defaultValue = default(T);
                }
                x = defaultValue;
            }
            return x;
        }

        public static T GetTOrThrowException<T>(this string input) where T : IConvertible
        {
            T x;
            try
            {
                x = (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception ex)
            {
                throw new ConversionException(input, typeof(T), ex);
            }
            return x;
        }

        [Serializable]
        public class ConversionException : Exception
        {
            public ConversionException()
            {
            }

            public ConversionException(string message) : base(message)
            {
            }

            public ConversionException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ConversionException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            public ConversionException(string toConvert, Type conversionTarget, Exception innerException) :
                base(
                    $"Conversion of value '{toConvert}' to type '{conversionTarget}' caused an Exception of type '{innerException.GetType()}'.",
                    innerException)
            {
            }
        }
    }

}
