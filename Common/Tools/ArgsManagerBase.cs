using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace Common.Tools
{
	public abstract class ArgsManagerBase
	{
		private Dictionary<string, string> args;

		public void Init(string[] args)
		{
			this.args = args.SelectPair((key, value) => new
				{
					Key = key,
					Value = value
				})
				.Where(kv => kv.Key.StartsWith("-"))
				.ToDictionary(kv => kv.Key.Substring(1), kv => kv.Value);
		}

		protected string GetValue(string key)
		{
			if (args == null)
				return null;

			if (!args.TryGetValue(key, out string value))
				return null;

			return value;
		}
	}
}