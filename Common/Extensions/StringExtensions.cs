using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
	public static class StringExtensions
    {
        public static string SJoin(this IEnumerable<string> strList, string delimiter = ", ")
        {
	        return string.Join(delimiter, strList.Where(v => v.IsNotNullOrEmpty()));
        }

        public static string SLineJoin(this IEnumerable<string> strList)
        {
	        return strList.SJoin("\r\n");
        }

        public static string SSpaceJoin(this IEnumerable<string> strList)
        {
	        return strList.SJoin(" ");
        }

        public static string ToLowerCamelCase(this string str)
        {
	        return string.IsNullOrEmpty(str) ? str : $"{str.Substring(0, 1).ToLower()}{str.Substring(1)}";
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
	        return !string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
	        return string.IsNullOrEmpty(str);
        }

        public static Dictionary<string, string> ToKeyValues(this string keyValues, char delimiter = ';', char subDelimiter = '=')
        {
	        return keyValues.Split(delimiter).Select(item => item.Split(subDelimiter)).ToDictionary(kv => kv[0], kv => kv[1]);
        }

        public static (string, string) SplitByHalf(this string str, string delimiter = "=")
        {
	        var pos = str.IndexOf(delimiter);
	        if (pos == -1)
		        return (str, null);

	        return (str.Substring(0, pos), str.Substring(pos + delimiter.Length));
        }

        public static Dictionary<string, string> ToKeyValues(this string keyValues, string delimiter = ";", string subDelimiter = "=")
        {
	        return keyValues.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
		        .Select(item => item.SplitByHalf(subDelimiter))
		        .ToDictionary(kv => kv.Item1.Trim(), kv => kv.Item2.Trim().RemoveParentheses());
        }

        public static string RemoveParentheses(this string str)
        {
	        var hasParentheses = (str.StartsWith("'") && str.EndsWith("'") || str.StartsWith("\"") && str.EndsWith("\"")) && str.Length > 1;

	        return hasParentheses ? str.Substring(1, str.Length - 2) : str;
        }

        public static TEnum? ToNEnum<TEnum>(this string str) where TEnum : struct, IConvertible
        {
	        return str.IsNullOrEmpty() ? (TEnum?)null : str.ToEnum<TEnum>();
        }

        public static TEnum ToEnum<TEnum>(this string str) where TEnum : struct, IConvertible
        {
	        return (TEnum)Enum.Parse(typeof(TEnum), str);
        }

        public static string Cut(this string str, int lenght)
        {
	        return str.Substring(0, Math.Min(str.Length, lenght));
        }
	}
}