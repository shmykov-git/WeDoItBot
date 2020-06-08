using System.IO;
using System.Linq;
using Bot.Model;
using NUnit.Framework;
using Suit;
using Suit.Extensions;
using Suit.Logs;

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

            maestro.Map = File.ReadAllText("Bots/bot.json").FromNamedJson<BotMap>();
            maestro.Actions = new[]
            {
                "/start",
                "/secondRoom",
                "/firstRoom",
                "Что дальше?"
            };

            maestro.Start();
        }

        [Test]
        public void KonfBotTest()
        {
            var maestro = IoC.Get<TestBotMaestro>();

            maestro.Map = File.ReadAllText("Bots/konfBot.json").FromNamedJson<BotMap>();
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