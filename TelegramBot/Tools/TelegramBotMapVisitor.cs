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
    [LoggingAspect(LoggingRule.Input | LoggingRule.Performance)]
    class TelegramBotMapVisitor: IBotMapVisitor
    {
        private readonly ILog log;
        private readonly ContentManager contentManager;
        private readonly TelegramUserContext context;

        private TelegramBotClient Bot => context.Bot;
        private long ChatId => context.Message.Chat.Id;

        public TelegramBotMapVisitor(ILog log, ContentManager contentManager, TelegramUserContext context)
        {
            this.log = log;
            this.contentManager = contentManager;
            this.context = context;
        }

        public Task VisitActionRoom(ActionRoom actionRoom)
        {
            log.Debug($"VisitActionRoom");

            return Task.CompletedTask;
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
                    InlineKeyboardButton.WithCallbackData(button.Name, $"{button.Key}"),
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
                            InlineKeyboardButton.WithCallbackData(button.Name, $"{button.Key}")).ToArray())
                        .ToArray()
                ));
        }

        public async Task VisitPicRoom(PicRoom picRoom)
        {
            log.Debug($"VisitPicRoom");
            await SendText(picRoom.Name);
            await SendPic(picRoom.Pic);
            await SendText(picRoom.Description);
        }


        private async Task SendText(string text)
        {
            if (text.IsNullOrEmpty())
                return;

            await Bot.SendTextMessageAsync(ChatId, text);
        }

        private async Task SendDialog(string text, InlineKeyboardMarkup keyboard)
        {
            await Bot.SendTextMessageAsync(ChatId, text, replyMarkup: keyboard);
        }

        private async Task SendPic(string pic)
        {
            if (!await contentManager.Apply(pic, async stream => await Bot.SendPhotoAsync(ChatId, new InputOnlineFile(stream))))
                log.Warn($"No pic {pic}");
        }

    }
}