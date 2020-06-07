using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;
using NLog.Config;
using Suit.Aspects;

namespace Suit.Logs
{
	public class LogByNLogAndConsole : ILog
	{
		public void Warn(string msg)
		{
            Console.WriteLine(msg);
			nLog.Warn(msg);
		}

		public void Exception(Exception e)
		{
            Console.WriteLine(e.ToString());
			nLog.Error(e);
		}

		public void Info(string msg)
		{
            Console.WriteLine(msg);
			nLog.Info(msg);
		}

		public void Debug(string msg)
		{
            Console.WriteLine(msg);
			nLog.Debug(msg);
		}

		public void Trace(string msg)
		{
            Console.WriteLine(msg);
			nLog.Trace(msg);
		}

		public void Error(string msg)
		{
            Console.WriteLine(msg);
			nLog.Error(msg);
		}

		public void Fatal(string msg)
		{
            Console.WriteLine(msg);
			nLog.Fatal(msg);
		}

		#region Configure nLog Magic

		private readonly Func<string, ILogger> loggerFactory;
		private ILogger _nLog = null;
		private ILogger nLog => _nLog ?? (_nLog = GetNLogLogger());

		public LogByNLogAndConsole(Func<string, ILogger> loggerFactory)
		{
			this.loggerFactory = loggerFactory;
		}

		public static void Configure(Assembly assembly)
		{
			var nlogConfigFile = GetEmbeddedResourceStream(assembly, "nlog.config");
			if (nlogConfigFile != null)
			{
				var xmlReader = System.Xml.XmlReader.Create(nlogConfigFile);
				NLog.LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
			}
		}

		public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
		{
			var resourcePaths = assembly.GetManifestResourceNames()
				.Where(x => x.EndsWith(resourceFileName, StringComparison.OrdinalIgnoreCase))
				.ToList();

			if (resourcePaths.Count == 1)
			{
				return assembly.GetManifestResourceStream(resourcePaths.Single());
			}

			return null;
		}

		private ILogger GetNLogLogger()
		{
			var stack = new StackTrace(false);
			var framesA = stack.GetFrames();
			if (framesA == null)
				return loggerFactory("Default");

			var frames = framesA.ToList();
			var currentFrame = frames.First(f => f.GetMethod().Name == nameof(GetNLogLogger));
			var currentFrameIndex = frames.IndexOf(currentFrame);
			var loggerFrame = frames[currentFrameIndex + 3];
			if (loggerFrame.GetMethod().DeclaringType == typeof(LoggingAspect))
				loggerFrame = frames[currentFrameIndex + 5];
			var loggerName = loggerFrame.GetMethod().DeclaringType?.FullName ?? "Default";

			if (loggerName.Contains("+"))
				loggerName = loggerName.Split('+')[0];

			return loggerFactory(loggerName);
		}

		#endregion
	}
}