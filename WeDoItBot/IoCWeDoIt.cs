using Common;
using Common.Tools;
using Unity;
using WeDoItBot.Tools;

namespace WeDoItBot
{
	public static class IoCWeDoIt
	{
		public static void Register(UnityContainer container)
		{
			container.RegisterSingleton<Settings>();
            container.RegisterFactory<IBotStarterSettings>(c => IoC.Get<Settings>());
            container.RegisterFactory<IJMasterSettings>(c => IoC.Get<Settings>());
		}
    }
}