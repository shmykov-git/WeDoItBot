using Suit;
using TelegramBot.Tools;
using Unity;

namespace TelegramBot
{
	public static class IoCTelegramBot
	{
		public static void Register(UnityContainer container)
		{
            container.RegisterSingleton<TelegramBotSettings>();
            container.RegisterFactory<IBotManagerSettings>(c => IoC.Get<TelegramBotSettings>());
        }
    }
}