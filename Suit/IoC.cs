using System;
using System.Reflection;
using NLog;
using Suit.Logs;
using Unity;
using Unity.Resolution;

namespace Suit
{
	public static class IoC
	{
		private static readonly UnityContainer Container = new UnityContainer();
		public static bool IsConfigured { get; private set; }

		public static T Get<T>(params ResolverOverride[] parameters)
		{
			return Container.Resolve<T>(parameters);
		}

		public static void Configure(params Action<UnityContainer>[] confugureActions)
		{
			LogByNLogAndConsole.Configure(Assembly.GetExecutingAssembly());
			Container.RegisterFactory<Func<string, ILogger>>(c => (Func<string, ILogger>)LogManager.GetLogger);
			Container.RegisterType<ILog, LogByNLogAndConsole>();

			foreach (var confugureAction in confugureActions)
			{
				confugureAction(Container);
			}

			IsConfigured = true;
		}
	}
}