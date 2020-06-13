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
            var settings = IoC.Get<BotTestSettings>();
            var maestro = IoC.Get<TestBotMaestro>();

            maestro.Map = File.ReadAllText(settings.TestBotFileName).ToBotMap();

            maestro.Actions = new[]
            {
                "/start",
                "/noTicket",
                "/service",
                "/konf",
                "Что дальше?"
            };

            maestro.Start();
        }
    }
}