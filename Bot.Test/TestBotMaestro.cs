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
                log.Debug($"");

                //todo: состояние и проверка артифактов

                if (action.StartsWith('/'))
                {
                    var roomKey = action.Substring(1);
                    log.Debug($"click>{roomKey}");
                    GoToRoom(roomKey);
                }
                else
                {
                    log.Debug($"type>{action}");
                }
            }
        }

        public Artifact[] GoToRoom(string roomKey)
        {
            log.Debug($"=> {roomKey}");

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