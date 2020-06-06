using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Common.Aspects;
using Common.Extensions;
using Common.Logs;
using Common.Tools;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using WeDoItBot.DataModel;
using File = System.IO.File;

namespace WeDoItBot.Tools
{
    [LoggingAspect(LoggingRule.Full)]
    class BotStarter
    {
        private readonly ILog log;
        private readonly IBotStarterSettings settings;
        private TelegramBotClient bot;
        private BotMap map;
        private ConcurrentDictionary<string, State> states = new ConcurrentDictionary<string, State>();
        private Func<State> defaultStateFn;

        private State GetState(string key) => states.GetOrAdd(key, k => defaultStateFn());
        private State GetState(Message message) => GetState(message.Chat.Username ?? message.Chat.Title);

        private int[] chats = {Chats.BotChat, Chats.BotTestChat};

        public BotStarter(ILog log, IBotStarterSettings settings)
        {
            this.log = log;
            this.settings = settings;
            map = System.IO.File.ReadAllText(settings.BotFile).FromJson<BotMap>();
            defaultStateFn = ()=>new State()
            {
                CurrentRoom = map.Rooms.First()
            };
        }

        [LoggingAspect(LoggingRule.Stabilize)]
        public async void Start()
        {
            var proxy = new WebProxy(settings.ProxyHost);
            bot = new TelegramBotClient(settings.BotToken, proxy);

            //var me = await bot.GetMeAsync();
            //Console.Title = me.Username;

            bot.OnMessage += BotOnMessageReceived;
            bot.OnReceiveGeneralError += (o, a) => Console.WriteLine($"GError:{a.Exception.Message}");
            bot.OnReceiveError += (o, a) => Console.WriteLine($"Error:{a.ApiRequestException.Message}");

            bot.StartReceiving();
            log.Info($"Start listening");

            //bot.GetMeAsync().ContinueWith(me=>{});

            //var image1 = File.Open(@"Content\bot.png", FileMode.Open);
            //bot.SendPhotoAsync(new ChatId(groupdChat), new InputOnlineFile(image1));
            //bot.SendTextMessageAsync(new ChatId(groupdChat), "Я опять включился, привет!");

            //bot.SendTextMessageAsync(new ChatId(400819276), "Подключился!");
            //var image2 = File.Open(@"Content\bot.png", FileMode.Open);
            //bot.SendPhotoAsync(new ChatId(400819276), new InputOnlineFile(image2));

            ShowRoom(Chats.BotChat, map.Rooms.First());
            //ShowRoom(Chats.BotTestChat, map.Rooms.First());

            Console.ReadLine();
            bot.StopReceiving();
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message.Type == MessageType.Text)
            {
                log.Info($"ChatId: {message.Chat.Id} Message: {message.Text}");

                if (e.Message.Text.StartsWith("/"))
                {
                    await DoCmd(message);

                    return;
                }

                var state = GetState(message);
                if (state.StateType == StateType.WaitingForAnswer)
                {
                    await GetAnswer(message);

                    return;
                }

                //await bot.SendChatActionAsync(e.Message.Chat.Id, ChatAction.Typing);

                //await Task.Delay(500);

                //if (e.Message.Text == "Test")
                //{
                //    await bot.SendTextMessageAsync(
                //        chatId: e.Message.Chat.Id,
                //        text: "Ищу нормальный прокси...",
                //        replyMarkup: InlineKeyboardMarkup.Empty()
                //    );
                //}

                //if (e.Message.Text.StartsWith("Вася пока"))
                //{
                //    await bot.SendTextMessageAsync(e.Message.Chat.Id, "Пока, остальное потом. И с прокси у меня беда, не очень стабилен");
                //    return;
                //}

                //if (e.Message.Text.StartsWith("Вася"))
                //{
                //    await bot.SendTextMessageAsync(e.Message.Chat.Id, "Я пока ничего не умею");
                //}

            }
        }

        private Task ShowRoom(Message message, Room room)
        {
            return ShowRoom(message.Chat.Id, room, message.Chat.Username ?? message.Chat.Title, message);
        }

        private async Task ShowRoom(long chatId, Room room, string username = null, Message message = null)
        {
            if (message == null)
            {
                await Say(chatId, "=====================================================");
            }

            await Say(chatId, room.Name);

            if (room.Pic.IsNotNullOrEmpty())
            {
                await ShowPic(chatId, room.Pic);
            }

            await Say(chatId, room.Description);
            await Say(chatId, room.Message);
            await Say(chatId, room.Message2);

            if (username.IsNotNullOrEmpty())
                GetState(username).CurrentRoom = room;

            foreach (var command in room.Commands)
            {
                await ShowCmd(message, command);

                if (command.Type == CommandType.Enter)
                    break;
            }
        }

        private async Task ShowCmd(Message message, Command commnad)
        {
            var chatId = message?.Chat?.Id??Chats.BotChat;
            State state = message == null ? null : GetState(message);

            switch (commnad.Type)
            {
                case CommandType.Link:
                case CommandType.Go:
                    await Say(chatId, $"/{commnad.Name}");
                    break;
                case CommandType.Ask:
                    await Say(chatId, commnad.Question);

                    if (commnad.NotImplMsg.IsNotNullOrEmpty())
                        await Say(chatId, $"[{commnad.NotImplMsg}]");

                    await Say(chatId, $"[/{commnad.NameYes}]");
                    await Say(chatId, $"[/{commnad.NameNo}]");
                    break;
                case CommandType.Enter:
                    await Say(chatId, commnad.Question);
                    state.StateType = StateType.WaitingForAnswer;
                    state.CurrentCommand = commnad;
                    break;
            }
        }

        private async Task Say(long chatId, string message, int? delay = null)
        {
            if (message.IsNullOrEmpty())
                return;

            if (delay.HasValue)
            {
                await bot.SendChatActionAsync(chatId, ChatAction.Typing);
                await Task.Delay(delay.Value);
            }

            await bot.SendTextMessageAsync(chatId, message);
        }

        private async Task GetAnswer(Message message)
        {
            var answer = message.Text;
            var state = GetState(message);

            state.Values.AddOrUpdate(state.CurrentCommand.ValueKey, k => answer, (k, v) => answer);

            var flag = false;
            foreach (var command in state.CurrentRoom.Commands)
            {
                if (flag)
                {
                    await ShowCmd(message, command);

                    if (command.Type == CommandType.Enter)
                        break;
                }
                else
                {
                    if (command == state.CurrentCommand)
                        flag = true;
                }
            }
        }

        private async Task DoCmd(Message message)
        {
            var cmd = message.Text.Substring(1).Split('@').First();

            if (cmd == "start")
            {
                await GoToRoom(message, "start");

                return;
            }

            var room = GetState(message).CurrentRoom;

            var goCommand = room.Commands.FirstOrDefault(c => c.Name == cmd);
            if (goCommand != null)
            {
                await GoToRoom(message, goCommand.Go);

                if (goCommand.Link.IsNotNullOrEmpty())
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, $"[Открою чат: {goCommand.Link}]");
                }
            }

            var yesCommand = room.Commands.FirstOrDefault(c => c.NameYes == cmd);
            if (yesCommand != null)
                await GoToRoom(message, yesCommand.GoYes);

            var noCommand = room.Commands.FirstOrDefault(c => c.NameNo == cmd);
            if (noCommand != null)
                await GoToRoom(message, noCommand.GoNo);

        }

        private async Task GoToLastCommandRoom(Message message)
        {
            var state = GetState(message);

            var goCommand = state.CurrentRoom.Commands.FirstOrDefault(c => c.Name == state.CurrentCommand.Go);
            if (goCommand != null)
            {
                await GoToRoom(message, goCommand.Go);
            }
        }

        private async Task GoToRoom(Message message, string roomKey)
        {
            await Say(message.Chat.Id, $"Перехожу в: {roomKey}");

            var newRoom = map.Rooms.FirstOrDefault(r => r.Key == roomKey);

            if (newRoom != null)
                await ShowRoom(message, newRoom);
        }

        private async Task ShowPic(long chatId, string imageKey)
        {
            var fileName = $@"Content\{imageKey}";
            log.Info($"Pic->{chatId}, {imageKey}");

            if (File.Exists(fileName))
            {
                //await bot.SendTextMessageAsync(chatId, $"+{imageKey}");

                //await bot.SendTextMessageAsync(chatId, $"[show pic {imageKey}]");

                ////todo: free memory
                var image = File.Open(fileName, FileMode.Open);
                await bot.SendPhotoAsync(chatId, new InputOnlineFile(image));
            }
            else
            {
                await bot.SendTextMessageAsync(chatId, $"no pic {imageKey}");
            }
        }
    }
}
