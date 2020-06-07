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
            var log = IoC.Get<ILog>();

            var map = File.ReadAllText("bot.json").FromNamedJson<BotMap>();
            var visitor = IoC.Get<TestBotMapVisitor>();

            var actions = new[]
            {
                "/start",
                "��� ������?"
            };

            void GoToRoom(string key)
            {
                var room = map.Rooms.First(r => r.Key == key);
                room.Visit(visitor);
                if (room.AutoGo.IsNotNullOrEmpty())
                    GoToRoom(room.AutoGo);
            }

            foreach (var action in actions)
            {
                if (action.StartsWith('/'))
                {
                    var cmd = action.Substring(1);
                    GoToRoom(cmd);
                }
                else
                {
                    // ����� ������ ��� ������ �� �����: �����, ����� �� ������ - ��� ������ ������������
                    log.Debug($"");
                    log.Debug($">{action}");
                }
            }

            //map.Visit(visitor);
        }
    }
}