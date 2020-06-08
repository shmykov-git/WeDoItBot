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
        public void ClickTest()
        {
            var maestro = IoC.Get<TestBotMaestro>();

            maestro.Map = File.ReadAllText("bot.json").FromNamedJson<BotMap>();
            maestro.Actions = new[]
            {
                "/start",
                "��� ������?"
            };

            maestro.Start();
        }
    }
}