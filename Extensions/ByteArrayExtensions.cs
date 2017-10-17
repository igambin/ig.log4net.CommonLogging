using System.Text;

namespace ig.log4net.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Tranform byte[] to string using specified encoding
        /// </summary>
        /// <typeparam name="TEncoding"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string AsString<TEncoding>(this byte[] arr) where TEncoding : Encoding, new() => new TEncoding().GetString(arr);

        /// <summary>
        /// Transform byte[] to string using default encoding: UTF8
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string AsString(this byte[] arr) => new UTF8Encoding().GetString(arr);
    }
}
