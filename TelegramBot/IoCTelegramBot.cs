using System;
using Suit;
using Suit.Logs;
using Telegram.Bot.Types;
using TelegramBot.Model;
using TelegramBot.Services;
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
            container.RegisterFactory<ITelegramBotServiceSettings>(c => IoC.Get<TelegramBotSettings>());

            container.RegisterSingleton<TelegramBotManager>();
            container.RegisterSingleton<TelegramBotService>();
            
            container.RegisterFactory<Func<SingleBot, TelegramUserContext>>(c => (Func<SingleBot, TelegramUserContext>) (bot =>
            {
                var context = new TelegramUserContext(IoC.Get<ILog>())
                {
                    Bot = bot,
                };

                context.Visitor = new TelegramBotMapVisitor(IoC.Get<ILog>(), IoC.Get<ContentManager>(), context);
                context.Maestro = new TelegramBotMaestro(IoC.Get<ILog>(), context);

                return context;
            }));

            container.RegisterSingleton<ContentManager>();
        }
    }
}