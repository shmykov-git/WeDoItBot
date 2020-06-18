using Bot.Extensions;
using Bot.Model;
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

                    Command(roomKey);
                }
                else
                {
                    Type(action);
                }
            }
        }

        public async void Command(string command)
        {
            log.Debug($"=> {command}");

            var room = Map.FindRoom(command);

            State.CurrentRoom = room;

            await room.Visit(visitor);

            if (room.AutoGo.IsNotNullOrEmpty())
                Command(room.AutoGo);
        }

        public void Type(string message)
        {
            log.Debug($"type>{message}");
        }
    }
}