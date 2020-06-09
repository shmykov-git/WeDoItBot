using System.IO;
using Bot.Extensions;
using NUnit.Framework;
using Suit;

namespace Bot.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            IoC.Configure(IoCBotTest.Register);
        }

        [Test]
        public void BotTest()
        {
            var maestro = IoC.Get<TestBotMaestro>();

            maestro.Map = File.ReadAllText("bot.json").ToBotMap();

            maestro.Actions = new[]
            {
                "/start",
                "/noTicket",
                "/hasTicket",
                "/konf",
                "Что дальше?"
            };

            maestro.Start();
        }
    }
}