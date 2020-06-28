using System;
using Suit;
using Suit.Logs;
using TelegramBot.Model;
using TelegramBot.Tools;
using Unity;

namespace TelegramBot
{
	public static class IoCTelegramBot
	{
        public static void Register(UnityContainer container)
        {
            container.RegisterSingleton<TelegramBotManager>();
            container.RegisterFactory<ITelegramBotManager>(c => IoC.Get<TelegramBotManager>());

            container.RegisterSingleton<ActionManager>();

            container.RegisterFactory<Func<SingleBot, TelegramUserContext>>(c => (Func<SingleBot, TelegramUserContext>) (bot =>
            {
                var context = new TelegramUserContext(IoC.Get<ILog>())
                {
                    Bot = bot,
                };

                context.Visitor = new TelegramBotMapVisitor(IoC.Get<ILog>(), IoC.Get<ContentManager>(), IoC.Get<ActionManager>(), context);
                context.Maestro = new TelegramBotMaestro(IoC.Get<ILog>(), context);

                return context;
            }));

            container.RegisterSingleton<ContentManager>();
        }
    }
}