using System.IO;
using Bot.Model;
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
            container.RegisterType<ILog, LogToConsole>();
            container.RegisterSingleton<IBotMapVisitor, TestBotMapVisitor>();

            container.RegisterSingleton<TestBotMaestro>();
            container.RegisterFactory<IBotMaestro>(c => IoC.Get<TestBotMaestro>());
        }
    }
}