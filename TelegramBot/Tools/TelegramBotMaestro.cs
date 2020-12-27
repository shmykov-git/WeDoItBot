using System.Collections.Concurrent;
using System.Linq;
using Bot.Extensions;
using Bot.Model;
using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;
using Bot.Model.Rooms.Simple;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;
using Telegram.Bot.Types;

namespace TelegramBot.Tools
{
    [LoggingAspect(LoggingRule.Input)]
    class TelegramBotMaestro: IBotMaestro
    {
        private readonly ILog log;
        private readonly TelegramUserContext context;

        private bool IsCommand(string message) => message.StartsWith("/");
        private string GetKey(string message) => message.Substring(1).Split('@').First();

        private Room GetRoom(string key) => context.Bot.Map.Rooms.FirstOrDefault(r => r.Key == key);
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
            if (!context.Bot.Map.RoomExists(command))
            {
                log.Warn($"There is no room {command}");

                return;
            }

            var room = context.Bot.Map.FindRoom(command);

            context.State.CurrentRoom = room;
            if (room is MenuRoom menuRoom)
                context.State.LastMenuRoom = menuRoom;

            context.State.StateType = StateType.None;

            await room.Visit(context.Visitor); 

            if (room is ShowRoom showRoom)
            {
                if (showRoom.EnterPlace != null)
                    context.State.StateType = StateType.WaitingForAnswer;
            }

            if (context.State.StateType != StateType.WaitingForAnswer && room.AutoGo.IsNotNullOrEmpty())
                Command(room.AutoGo);
        }

        public async void Type(string message)
        {
            if (IsCommand(message))
            {
                Command(GetKey(message));

                return;
            }

            var replyGoCommand = context.State.LastMenuRoom?.FindReplyGo(message) ??
                                 context.Bot.Map.FindReplyGo(message);
            if (replyGoCommand.IsNotNullOrEmpty())
            {
                Command(replyGoCommand);

                return;
            }

            if (context.State.StateType == StateType.WaitingForAnswer)
            {
                var room = (ShowRoom)context.State.CurrentRoom;

                if (room.EnterPlace.Type != EnterType.Text)
                    return;

                var key = room.EnterPlace.Key;

                context.State.Values.TryAdd(key, message);

                await room.Visit(context.Visitor);

                if (room.EnterPlace == null)
                    context.State.StateType = StateType.None;

                if (context.State.StateType != StateType.WaitingForAnswer && room.AutoGo.IsNotNullOrEmpty())
                    Command(room.AutoGo);
            }
        }

        public async void Photo(string fileName)
        {
            if (context.State.StateType == StateType.WaitingForAnswer)
            {
                var room = (ShowRoom)context.State.CurrentRoom;

                if (room.EnterPlace.Type != EnterType.Photo)
                    return;

                var key = room.EnterPlace.Key;

                context.State.Values.AddOrUpdate(key, k => fileName, (k, v) => fileName);

                await room.Visit(context.Visitor);

                if (room.EnterPlace == null)
                    context.State.StateType = StateType.None;

                if (room.AutoGo.IsNotNullOrEmpty())
                    Command(room.AutoGo);
            }
        }
    }
}