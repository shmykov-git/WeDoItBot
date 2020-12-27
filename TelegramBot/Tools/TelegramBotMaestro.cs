using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
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
            log.Debug($"#Found room: {room.Id} {{{room.GetType().Name}}}");

            CleanRoomState(context.State.CurrentRoom);

            context.State.CurrentRoom = room;

            await room.Visit(context.Visitor);

            SetRoomState(room);

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

            await CheckRoomAnswer(message, EnterType.Text);
        }

        public async void Photo(string fileName)
        {
            await CheckRoomAnswer(fileName, EnterType.Photo);
        }

        private async Task CheckRoomAnswer(string value, EnterType type)
        {
            if (context.State.StateType != StateType.WaitingForAnswer)
                return;

            var room = (ShowRoom)context.State.CurrentRoom;

            if (room.EnterPlace.Type != type)
                return;

            var key = room.EnterPlace.Key;

            context.State.Values.AddOrUpdate(key, k => value, (k, v) => value);

            await room.Visit(context.Visitor);

            if (room.EnterPlace == null)
                context.State.StateType = StateType.None;

            if (context.State.StateType != StateType.WaitingForAnswer && room.AutoGo.IsNotNullOrEmpty())
                Command(room.AutoGo);
        }

        private void CleanRoomState(Room room)
        {
            if (room == null)
                return;

            context.State.StateType = StateType.None;

            if (room is ShowRoom showRoom)
                showRoom.EnterPlace = null;
        }

        private void SetRoomState(Room room)
        {
            if (room is MenuRoom menuRoom)
                context.State.LastMenuRoom = menuRoom;

            if (room is ShowRoom showRoom)
            {
                if (showRoom.EnterPlace != null)
                    context.State.StateType = StateType.WaitingForAnswer;
            }
        }
    }
}