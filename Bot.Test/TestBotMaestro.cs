using System;
using System.Linq;
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

        private State State { get; } = new State();

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

                if (action.StartsWith('/'))
                {
                    var roomKey = action.Substring(1);
                    log.Debug($"click>{roomKey}");

                    if (!State.CanGo(roomKey))
                        throw new ApplicationException($"No artifact {roomKey}");

                    GoToRoom(roomKey);
                }
                else
                {
                    Type(action);
                }
            }
        }

        public void GoToRoom(string roomKey)
        {
            log.Debug($"=> {roomKey}");

            var room = Map.FindRoom(roomKey);

            State.CurrentRoom = room;

            room.Visit(visitor);

            if (room.AutoGo.IsNotNullOrEmpty())
                GoToRoom(room.AutoGo);
        }

        public void Type(string message)
        {
            log.Debug($"type>{message}");
        }
    }
}