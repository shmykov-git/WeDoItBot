using Starter.Tools;
using Suit;
using TelegramBot.Services;
using TelegramBot.Tools;
using Unity;

namespace Starter
{
	public static class IoCStarter
	{
        public static void Register(UnityContainer container)
        {
            container.RegisterSingleton<StarterSettings>();
            container.RegisterFactory<ITelegramBotManagerSettings>(c => IoC.Get<StarterSettings>());
            //container.RegisterFactory<ITelegramBotServiceSettings>(c => IoC.Get<StarterSettings>());

            container.RegisterSingleton<TelegramBotService>();

            container.RegisterType<IActionManagerSettings, BotActionManager>();
        }
    }
}