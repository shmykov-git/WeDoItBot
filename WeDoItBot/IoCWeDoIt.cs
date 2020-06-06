using Common;
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
		}
    }
}