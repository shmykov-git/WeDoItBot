using System;
using System.IO;
using Common.Extensions;
using Common.Logs;

namespace Common.Tools
{
	public class FileManager
	{
		private readonly ILog log;

		public FileManager(ILog log)
		{
			this.log = log;
		}

		public bool TryReadAllBytes(string fileName, out byte[] bytes)
		{
			try
			{
				bytes = File.ReadAllBytes(fileName);

				return true;
			}
			catch (Exception e)
			{
				log.Exception(e);
				bytes = null;

				return false;
			}
		}

		public bool WriteAllBytes(string fileName, byte[] bytes)
		{
			try
			{
				File.WriteAllBytes(fileName, bytes);

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);

				return false;
			}
		}

		public void CreateFolderIfNotExists(string folder)
		{
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);
		}

		public string[] GetFiles(string folder, string searchPattern = "*.*")
		{
			return Directory.GetFiles(folder, searchPattern, SearchOption.AllDirectories);
		}

		public string[] GetDirectories(string folder)
		{
			return Directory.GetDirectories(folder);
		}

		public T ReadJson<T>(string fileName)
		{
			var json = File.ReadAllText(fileName);

			return json.FromJson<T>();
		}

		public T ReadNamedJson<T>(string fileName)
		{
			var json = File.ReadAllText(fileName);

			return json.FromNamedJson<T>();
		}

		public string ReadText(string fileName)
		{
			return File.ReadAllText(fileName);
		}
	}
}