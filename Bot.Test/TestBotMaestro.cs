using System;
using Bot.Extensions;
using Bot.Model;
using Bot.Model.Artifacts;
using Suit.Extensions;
using Suit.Logs;

namespace Bot.Test
{
    public class TestBotMaestro : IBotMaestro
    {
        private readonly ILog log;
        private readonly IBotMapVisitor visitor;

        public string[] Actions { get; set; }
        public BotMap Map { get; set; }

        public TestBotMaestro(ILog log, IBotMapVisitor visitor)
        {
            this.log = log;
            this.visitor = visitor;
        }

        public void Start()
        {
            foreach (var action in Actions)
            {
                if (action.StartsWith('/'))
                {
                    var roomKey = action.Substring(1);
                    GoToRoom(roomKey);
                }
                else
                {
                    log.Debug($"");
                    log.Debug($">{action}");
                }
            }
        }

        public Artifact[] GoToRoom(string roomKey)
        {
            var room = Map.FindRoom(roomKey);
            if (room == null)
                throw new ApplicationException($"No room {roomKey}");

            room.Visit(visitor);

            if (room.AutoGo.IsNotNullOrEmpty())
                GoToRoom(room.AutoGo);

            return room.Artifacts;
        }

        public void Say(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}