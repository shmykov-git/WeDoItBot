using System.Collections.Concurrent;
using System.Linq;
using Bot.Extensions;
using Bot.Model;
using Bot.Model.Rooms;
using Suit.Extensions;
using Suit.Logs;
using Telegram.Bot.Types;

namespace TelegramBot.Tools
{
    class TelegramBotMaestro: IBotMaestro
    {
        private readonly ILog log;
        private readonly TelegramUserContext context;

        private bool IsCommand(string message) => message.StartsWith("/");
        private string GetKey(string message) => message.Substring(1).Split('@').First();

        private Room GetRoom(string key) => context.Map.Rooms.FirstOrDefault(r => r.Key == key);
        private bool IsWaitingForAnswer(Message message) => context.State.StateType == StateType.WaitingForAnswer;

        public TelegramBotMaestro(ILog log, TelegramUserContext context)
        {
            this.log = log;
            this.context = context;
        }

        public void Start()
        {
            Command("start");
        }

        public async void Command(string command)
        {
            log.Debug($"#Command: {command}");
            var room = context.Map.FindRoom(command);

            context.State.CurrentRoom = room;

            await room.Visit(context.Visitor);

            if (room.AutoGo.IsNotNullOrEmpty())
                Command(room.AutoGo);
        }

        public void Type(string message)
        {
            log.Debug($"#Type: {message}");
            if (IsCommand(message))
            {
                Command(GetKey(message));

                return;
            }

            // type
        }
    }
}