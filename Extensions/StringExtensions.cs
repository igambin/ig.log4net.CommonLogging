using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ig.log4net.Extensions
{
    public static class StringExtensions
    {

        public static string AddIfNotNull(this string input, string toAdd) => input == null ? "" : input + toAdd;

        public static byte[] ToByteArray(this string str) => new UTF8Encoding().GetBytes(str);

        public static byte[] ToByteArray<TEncoding>(this string str) where TEncoding : Encoding, new() => new TEncoding().GetBytes(str);

        public static string PrependNewLine(this string input, int count = 1) => string.Join("", Enumerable.Range(1, count).ToList().Select(x => Environment.NewLine)) + input;

        public static string IndentLine(this string input, int spaces = 8) => new string(' ', spaces) + input;

        public static string Append(this string input, string toAppend) => input + toAppend;

        public static string Prepend(this string input, string toPrepend) => toPrepend + input;

        public static string AppendNewLine(this string input, int count = 1) => input + string.Join("", Enumerable.Range(1, count).ToList().Select(x => Environment.NewLine));

        public static string AppendLine(this string input, string toAppend = "") => input + toAppend + Environment.NewLine;

        public static string PrependLine(this string input, string toPrepend = "") => Environment.NewLine + toPrepend + input;

        public static string MatchKey(this string haystack, Regex pattern, string needle)
        {
            var matches = pattern.Match(haystack);
            return matches.Success ? matches.Groups[needle].Value : $"no{needle}";
        }
    }
}
