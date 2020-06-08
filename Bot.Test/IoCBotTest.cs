using System.IO;
using Bot.Model;
using Bot.Tools;
using Suit;
using Suit.Extensions;
using Suit.Logs;
using Unity;

namespace Bot.Test
{
    public static class IoCBotTest
    {
        public static void Register(UnityContainer container)
        {
            container.RegisterType<ILog, LogToDebugAndConsole>();
            container.RegisterSingleton<IBotMapVisitor, BotMapLogger>();

            container.RegisterSingleton<TestBotMaestro>();
            container.RegisterFactory<IBotMaestro>(c => IoC.Get<TestBotMaestro>());
        }
    }
}