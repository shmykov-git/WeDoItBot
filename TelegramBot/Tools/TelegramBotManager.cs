using System;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bot.Model;
using Bot.Model.Rooms;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace TelegramBot.Tools
{
    [LoggingAspect(LoggingRule.Full)]
    class TelegramBotManager
    {
        private readonly ILog log;
        private readonly IBotManagerSettings settings;
        private BotMap botMap;
        private TelegramBotClient bot;

        private ConcurrentDictionary<string, State> states = new ConcurrentDictionary<string, State>();

        private State GetState(string key) => states.GetOrAdd(key, k => new State());
        private State GetState(Message message) => GetState(message.Chat.Username ?? message.Chat.Title);
        private bool IsCommand(Message message) => message.Text.StartsWith("/");
        private string GetKey(Message message) => message.Text.Substring(1).Split('@').First();
        private Room GetRoom(string key) => botMap.Rooms.FirstOrDefault(r => r.Key == key);
        private bool IsWaitingForAnswer(Message message) => GetState(message).StateType == StateType.WaitingForAnswer;

        public TelegramBotManager(ILog log, IBotManagerSettings settings)
        {
            this.log = log;
            this.settings = settings;
            botMap = File.ReadAllText(settings.BotMapFile).FromJson<BotMap>();
        }

        public async void Start()
        {
            var proxy = new WebProxy(settings.ProxyHost);
            bot = new TelegramBotClient(settings.BotToken, proxy);


            //bot.OnMessage += (o, a) => OnMessage(a.Message);
            //bot.OnReceiveGeneralError += (o, a) => log.Error($"GError:{a.Exception.Message}");
            //bot.OnReceiveError += (o, a) => log.Error($"Error:{a.ApiRequestException.Message}");
            //bot.OnCallbackQuery += async (o, a) => await DoCmd(a.CallbackQuery.Message);

            bot.StartReceiving();
            log.Info($"Start listening");

            Console.ReadLine();
            bot.StopReceiving();
        }

        //private async void OnMessage(Message message)
        //{
        //    switch (message.Type)
        //    {
        //        case MessageType.Text:
        //            await OnTextMessage(message);
        //            break;
        //    }
        //}

        //private Task OnTextMessage(Message message)
        //{
        //    log.Info($"ChatId: {message.Chat.Id} Message: {message.Text}");

        //    if (IsCommand(message))
        //    {
        //        return DoCmd(message);
        //    }

        //    if (IsWaitingForAnswer(message))
        //    {
        //        return GetAnswer(message);
        //    }

        //    return Task.CompletedTask;
        //}

        //private async Task Say(Message message, string text, int? delay = null)
        //{
        //    if (text.IsNullOrEmpty())
        //        return;

        //    if (delay.HasValue)
        //    {
        //        await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        //        await Task.Delay(delay.Value);
        //    }

        //    await bot.SendTextMessageAsync(message.Chat.Id, text);
        //}

        //private async Task DoCmd(Message message)
        //{
        //    var key = GetKey(message);

        //    var room = GetRoom(key);
        //    if (room == null)
        //    {
        //        await GoToRoom(message, key);
        //        return;
        //    }

        //    // todo: this is another type command
        //}











        //private Task ShowRoom(Message message, Room room)
        //{
        //    return ShowRoom(message.Chat.Id, room, message.Chat.Username ?? message.Chat.Title, message);
        //}

        //private async Task ShowRoom(long chatId, Room room, string username = null, Message message = null)
        //{
        //    if (message == null)
        //    {
        //        await Say(chatId, "=====================================================");
        //    }

        //    await Say(chatId, room.Name);

        //    if (room.Pic.IsNotNullOrEmpty())
        //    {
        //        await ShowPic(chatId, room.Pic);
        //    }

        //    await Say(chatId, room.Description);
        //    await Say(chatId, room.Message);
        //    await Say(chatId, room.Message2);

        //    if (username.IsNotNullOrEmpty())
        //        GetState(username).CurrentRoom = room;

        //    foreach (var command in room.Commands)
        //    {
        //        await ShowCmd(message, command);

        //        if (command.Type == CommandType.Enter)
        //            break;
        //    }
        //}

        //private async Task ShowCmd(Message message, Command commnad)
        //{
        //    var chatId = message.Chat.Id;
        //    var state = GetState(message);

        //    switch (commnad.Type)
        //    {
        //        case CommandType.Link:
        //        case CommandType.Go:
        //            var keyboard = new InlineKeyboardMarkup(new[]
        //            {
        //                new [] 
        //                {
        //                    InlineKeyboardButton.WithCallbackData(commnad.Name, $"/{commnad.Name}"),
        //                }
        //            });
                   
        //            await bot.SendTextMessageAsync(chatId, $"/{commnad.Name}", replyMarkup: keyboard);
        //            //await Say(chatId, $"/{commnad.Name}");
        //            break;
        //        case CommandType.Ask:
        //            await Say(chatId, commnad.Question);

        //            if (commnad.NotImplMsg.IsNotNullOrEmpty())
        //                await Say(chatId, $"[{commnad.NotImplMsg}]");

        //            await Say(chatId, $"[/{commnad.NameYes}]");
        //            await Say(chatId, $"[/{commnad.NameNo}]");
        //            break;
        //        case CommandType.Enter:
        //            await Say(chatId, commnad.Question);
        //            state.StateType = StateType.WaitingForAnswer;
        //            state.CurrentCommand = commnad;
        //            break;
        //    }
        //}

        //private async Task Say(long chatId, string message, int? delay = null)
        //{
        //    if (message.IsNullOrEmpty())
        //        return;

        //    if (delay.HasValue)
        //    {
        //        await bot.SendChatActionAsync(chatId, ChatAction.Typing);
        //        await Task.Delay(delay.Value);
        //    }

        //    await bot.SendTextMessageAsync(chatId, message);
        //}

        //private async Task GetAnswer(Message message)
        //{
        //    var answer = message.Text;
        //    var state = GetState(message);

        //    state.Values.AddOrUpdate(state.CurrentCommand.ValueKey, k => answer, (k, v) => answer);

        //    var flag = false;
        //    foreach (var command in state.CurrentRoom.Commands)
        //    {
        //        if (flag)
        //        {
        //            await ShowCmd(message, command);

        //            if (command.Type == CommandType.Enter)
        //                break;
        //        }
        //        else
        //        {
        //            if (command == state.CurrentCommand)
        //                flag = true;
        //        }
        //    }
        //}



        //private async Task GoToLastCommandRoom(Message message)
        //{
        //    var state = GetState(message);

        //    var goCommand = state.CurrentRoom.Commands.FirstOrDefault(c => c.Name == state.CurrentCommand.Go);
        //    if (goCommand != null)
        //    {
        //        await GoToRoom(message, goCommand.Go);
        //    }
        //}

        //private async Task GoToRoom(Message message, string key = null)
        //{
        //    var roomKey = key ?? GetKey(message);

        //    await Say(message.Chat.Id, $"Перехожу в: {roomKey}");

        //    var newRoom = botMap.Rooms.FirstOrDefault(r => r.Key == roomKey);

        //    if (newRoom != null)
        //        await ShowRoom(message, newRoom);
        //}

        //private async Task ShowPic(long chatId, string imageKey)
        //{
        //    var fileName = $@"Content\{imageKey}";
        //    log.Info($"Pic->{chatId}, {imageKey}");

        //    if (File.Exists(fileName))
        //    {
        //        //await bot.SendTextMessageAsync(chatId, $"+{imageKey}");
        //        //await bot.SendTextMessageAsync(chatId, $"[show pic {imageKey}]");

        //        using (var image = File.Open(fileName, FileMode.Open))
        //        {
        //            await bot.SendPhotoAsync(chatId, new InputOnlineFile(image));
        //        }

        //        //var image = File.Open(fileName, FileMode.Open);
        //        //await bot.SendPhotoAsync(chatId, new InputOnlineFile(image));
        //    }
        //    else
        //    {
        //        await bot.SendTextMessageAsync(chatId, $"no pic {imageKey}");
        //    }
        //}
    }
}
