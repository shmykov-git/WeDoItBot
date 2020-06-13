using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bot.Extensions;
using Bot.Model;
using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Tools
{
    [LoggingAspect(LoggingRule.Input)]
    class TelegramBotMapVisitor: IBotMapVisitor
    {
        private readonly ILog log;
        private readonly ContentManager contentManager;
        private readonly TelegramUserContext context;

        private TelegramBotClient Client => context.Bot.Client;
        private long ChatId => context.Message.Chat.Id;

        public TelegramBotMapVisitor(ILog log, ContentManager contentManager, TelegramUserContext context)
        {
            this.log = log;
            this.contentManager = contentManager;
            this.context = context;
        }

        public async Task VisitActionRoom(ActionRoom actionRoom)
        {
            log.Debug($"VisitActionRoom");

            switch (actionRoom.ActionName)
            {
                case "SendConfig":
                    await SendText(context.Bot.BotConfig);
                    break;
            }
        }

        public Task VisitRoom(Room room)
        {
            log.Debug($"VisitRoom");

            return Task.CompletedTask;
        }

        public Task VisitActionRoomPlace(ActionRoomPlace actionRoomPlace)
        {
            log.Debug($"VisitActionRoomPlace");

            return Task.CompletedTask;
        }

        public async Task VisitButton(Button button)
        {
            log.Debug($"VisitButton");
            await SendDialog(button.Caption, new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(button.Name, $"{button.Go}"),
                }
            }));
        }

        public async Task VisitButtonDialog(ButtonDialog buttonDialog)
        {
            log.Debug($"VisitButtonDialog");

            await SendDialog(buttonDialog.Caption,
                new InlineKeyboardMarkup(
                    buttonDialog.Buttons
                        .ByIndex()
                        .GroupBy(i => i / buttonDialog.ColumnsCount)
                        .Select(gi => gi.Select(i => buttonDialog.Buttons[i]).Select(button =>
                            InlineKeyboardButton.WithCallbackData(button.Name, $"{button.Go}")).ToArray())
                        .ToArray()
                ));
        }

        public async Task VisitButtonMenu(ButtonMenu buttonDialog)
        {
            log.Debug($"VisitButtonMenu");

            await SendDialog(buttonDialog.Caption,
                new ReplyKeyboardMarkup(
                    buttonDialog.Buttons
                        .ByIndex()
                        .GroupBy(i => i / buttonDialog.ColumnsCount)
                        .Select(gi => gi.Select(i => buttonDialog.Buttons[i]).Select(button =>
                            new KeyboardButton(button.Go)).ToArray())
                        .ToArray(), 
                    true
                ));
        }

        public async Task EnterPlace(EnterPlace enterPlace)
        {
            log.Debug($"EnterPlace");

            await SendText(enterPlace.Name);
        }

        public async Task VisitPicRoom(PicRoom picRoom)
        {
            log.Debug($"VisitPicRoom");
            await SendText(picRoom.Name);
            await SendPic(picRoom.Pic);
            await SendText(picRoom.Caption);
        }


        private async Task SendText(string text)
        {
            if (text.IsNullOrEmpty())
                return;

            await Client.SendTextMessageAsync(ChatId, text);
        }

        private async Task SendDialog(string text, IReplyMarkup replyMarkup)
        {
            await Client.SendTextMessageAsync(ChatId, text, replyMarkup: replyMarkup);
        }

        private async Task SendPic(string pic)
        {
            if (!await contentManager.Apply(pic, async stream => await Client.SendPhotoAsync(ChatId, new InputOnlineFile(stream))))
                log.Warn($"No pic {pic}");
        }

    }
}