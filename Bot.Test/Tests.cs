using System.IO;
using Bot.Model;
using NUnit.Framework;
using Suit;
using Suit.Extensions;

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
        public void Test1()
        {
            var map = File.ReadAllText("bot.json").FromNamedJson<BotMap>();
            var visitor = IoC.Get<TestBotMapVisitor>();

            map.Visit(visitor);
        }
    }
}