using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
	public static class DictionaryExtensions
	{
		public static ConcurrentDictionary<TKey, TValue> ToConcurrent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			return new ConcurrentDictionary<TKey, TValue>(dictionary);
		}

		public static string ToKeyValueStr<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, string delimiter = "\r\n", string subDelimiter = "=")
		{
			return dictionary.Select(kv=>$"{kv.Key.ToString()}{subDelimiter}{kv.Value.ToString()}").SJoin(delimiter);
		}

	}
}