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
        public void WeDoItTest()
        {
            var settings = IoC.Get<BotTestSettings>();
            var maestro = IoC.Get<TestBotMaestro>();

            maestro.Map = File.ReadAllText(settings.TestWeDoItBotFileName).ToBotMap();

            maestro.Actions = maestro.Map.Rooms.Select(r => r.Key)
                .Concat(maestro.Map.Rooms.SelectMany(r => r.GoList))
                .Distinct()
                .Select(key => $"/{key}")
                .ToArray();

            maestro.Start();
        }

        [Test]
        public void HowToTest()
        {
            var settings = IoC.Get<BotTestSettings>();
            var maestro = IoC.Get<TestBotMaestro>();

            maestro.Map = File.ReadAllText(settings.TestHowToBotFileName).ToBotMap();

            maestro.Actions = maestro.Map.Rooms.Select(r => r.Key)
                .Concat(maestro.Map.Rooms.SelectMany(r => r.GoList))
                .Distinct()
                .Select(key => $"/{key}")
                .ToArray();

            maestro.Start();
        }
    }
}