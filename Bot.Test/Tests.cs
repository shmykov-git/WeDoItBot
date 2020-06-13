using System.IO;
using System.Linq;
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

        [Test]
        public void HowToTest()
        {
            var settings = IoC.Get<BotTestSettings>();
            var maestro = IoC.Get<TestBotMaestro>();

            maestro.Map = File.ReadAllText(settings.TestBotHowToFileName).ToBotMap();

            maestro.Actions = maestro.Map.Rooms.Select(r => $"/{r.Id}")
                .Concat(maestro.Map.Rooms.Where(r => r.AutoGo != null).Select(r => r.AutoGo))
                .ToArray();

            maestro.Start();
        }
    }
}