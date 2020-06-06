using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logs;
using Newtonsoft.Json.Linq;

namespace Common.Tools
{
	public class JMaster
	{
		private readonly ILog log;
		private readonly IJMasterSettings settings;
		private readonly FileManager fileManager;

		private JObject translations;

		public JMaster(ILog log, IJMasterSettings settings, FileManager fileManager)
		{
			this.log = log;
			this.settings = settings;
			this.fileManager = fileManager;

			translations = fileManager.ReadJson<JObject>(settings.JMasterFileName);
		}

		public Dictionary<string, string> GetDictionary(string key)
		{
			var part = MoveByKey(key.Split('.'));

			return part.Properties().Where(p => p.Value.Type == JTokenType.String)
				.ToDictionary(p => p.Name, p => p.Value.ToString());
		}

        public T Get<T>(string key)
        {
            var part = MoveByKey(key.Split('.'));

            return part.ToObject<T>();
        }

		public string Get(string key)
		{
			var keyParts = key.Split('.');
			var part = MoveByKey(keyParts.Take(keyParts.Length - 1));

			var jToken = part.GetValue(keyParts[keyParts.Length - 1]);

			if (jToken.Type != JTokenType.String)
				throw new ArgumentException(keyParts[keyParts.Length - 1]);

			return jToken.ToString();
		}

		private JObject MoveByKey(IEnumerable<string> keys)
		{
			var part = translations;

			foreach (var keyPart in keys)
			{
				if (!part.TryGetValue(keyPart, out JToken jToken))
					throw new ArgumentException(keyPart);

				if (!(jToken is JObject jObject))
					throw new ApplicationException("Incorrect translation file");

				part = jObject;
			}

			return part;
		}
	}
}