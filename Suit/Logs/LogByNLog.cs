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
	public class LogByNLog : ILog
	{
		public void Warn(string msg)
		{
			nLog.Warn(msg);
		}

		public void Exception(Exception e)
		{
			nLog.Error(e);
		}

		public void Info(string msg)
		{
			nLog.Info(msg);
		}

		public void Debug(string msg)
		{
			nLog.Debug(msg);
		}

		public void Trace(string msg)
		{
			nLog.Trace(msg);
		}

		public void Error(string msg)
		{
			nLog.Error(msg);
		}

		public void Fatal(string msg)
		{
			nLog.Fatal(msg);
		}

		#region Configure nLog Magic

		private readonly Func<string, ILogger> loggerFactory;
		private ILogger _nLog = null;
		private ILogger nLog => _nLog ?? (_nLog = GetNLogLogger());

		public LogByNLog(Func<string, ILogger> loggerFactory)
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