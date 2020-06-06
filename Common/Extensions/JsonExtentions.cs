using System;
using Common.Logs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Extensions
{
	public static class JsonExtensions
	{
		private static ILog log = IoC.Get<ILog>();

		public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All
		};

		public static string ToJsonStr(this object obj, Formatting formatting = Formatting.None)
		{
			return JsonConvert.SerializeObject(obj, formatting);
		}

		public static string ToJsonNamedStr(this object obj, Formatting formatting = Formatting.None)
		{
			return JsonConvert.SerializeObject(obj, formatting, JsonSerializerSettings);
		}

		public static T FromJson<T>(this string json)
		{
			return (T)JsonConvert.DeserializeObject<T>(json);
		}

		public static T FromNamedJson<T>(this string json)
		{
			return (T)JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);
		}

		public static JArray ToJsonArray(this object obj)
        {
            return JArray.FromObject(obj);
        }

        public static JObject ToJson(this object obj)
        {
	        return JObject.FromObject(obj);
        }

        public static string ToJsonForLog(this object obj, bool noCut = false)
        {
	        var json = obj.ToJsonStr();

	        if (noCut)
		        return json;

	        return json.Length <= 1024 ? json : $"{json.Substring(0, 1024 - 3)}...";
        }
	}
}
