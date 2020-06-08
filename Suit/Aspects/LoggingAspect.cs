using System;
using System.Diagnostics;
using System.Reflection;
using AspectInjector.Broker;
using Suit.Extensions;
using Suit.Logs;

namespace Suit.Aspects
{
	[Aspect(Scope.Global)]
	[Injection(typeof(LoggingAspect))]
    //todo: doesn't work for .net core
	public class LoggingAspect : Attribute
	{
		private readonly LoggingRule loggingRule;

		private ILog _log;
		private ILog log => _log ?? (_log = IoC.Get<ILog>());

		private bool NoCut => loggingRule.HasFlag(LoggingRule.NoCut);
		private bool Stabilize => loggingRule.HasFlag(LoggingRule.Stabilize);

		private bool IsExceptionLog => loggingRule.HasFlag(LoggingRule.Exception);
		private bool IsArgsLog => loggingRule.HasFlag(LoggingRule.Args);
		private bool IsResultLog => loggingRule.HasFlag(LoggingRule.Result);
		private bool IsPerformanceLog => loggingRule.HasFlag(LoggingRule.Performance);

		public LoggingAspect() :this (LoggingRule.Default)
		{ }

		public LoggingAspect(LoggingRule loggingRule)
		{
			this.loggingRule = loggingRule;
		}

		private LoggingAspect FindMe(MethodBase method) => (method.GetCustomAttribute<LoggingAspect>() ??
		                                                   method.DeclaringType.GetCustomAttribute<LoggingAspect>()) ??
		                                                   method.DeclaringType.Assembly.GetCustomAttribute<LoggingAspect>();

		[Advice(Kind.Around, Targets = Target.Method)]
		public object Decorator(
			[Argument(Source.Metadata)] MethodBase methodInfo,
			[Argument(Source.Target)] Func<object[], object> method,
			[Argument(Source.Arguments)] object[] args)
		{
			if (!IoC.IsConfigured)
				return method(args);

			return FindMe(methodInfo).Around(methodInfo, method, args);
		}

		private object Around(MethodBase methodInfo, Func<object[], object> method, object[] args)
		{
			if (IsArgsLog)
				try
				{
					var methodParameters = methodInfo.GetParameters();
					var methodArgs = args.SelectByIndex((i, a) => $"{methodParameters[i].Name}: {GetValue(a)}").SJoin();
					log.Debug($"{(IsResultLog ? " ==>" : " == ")}{methodInfo.Name} {methodArgs}");
				}
				catch (Exception e)
				{
					log.Exception(e);
				}

			object result = null;

			var performanceCounter = IsPerformanceLog ? Stopwatch.StartNew() : null;

			try
			{
				result = method(args);
			}
			catch (Exception methodException)
			{
				if (IsExceptionLog)
					log.Exception(methodException);

				if (!Stabilize)
					throw;
			}
			finally
			{
				if (performanceCounter != null)
				{
					performanceCounter.Stop();
					log.Debug($"$$$ {methodInfo.Name}: {performanceCounter.ElapsedMilliseconds}ms");
				}
			}

			if (IsResultLog)
				try
				{
					var res = result?.ToJsonForLog(NoCut);
					log.Debug($"<== {methodInfo.Name}: {res}");
				}
				catch (Exception e)
				{
					log.Exception(e);
				}

			return result;
		}

		private string GetValue(object a)
		{
			if (a == null)
				return "";

			try
			{
				return a.ToJsonForLog(NoCut);
			}
			catch (Exception e)
			{
				log.Exception(e);
				return a.ToString();
			}
		}
	}
}
