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
        private readonly TelegramUserContext context;

        private TelegramBotClient Bot => context.Bot;
        private long ChatId => context.Message.Chat.Id;

        public TelegramBotMapVisitor(ILog log, TelegramUserContext context)
        {
            this.log = log;
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
            await SendDialog(buttonDialog.Question,
                new InlineKeyboardMarkup(new[]
                {
                    buttonDialog.Buttons.Select(button =>
                        InlineKeyboardButton.WithCallbackData(button.Name, $"{button.Key}")).ToArray()
                }));
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
            await Bot.SendTextMessageAsync(ChatId, text);
        }

        private async Task SendDialog(string text, InlineKeyboardMarkup keyboard)
        {
            await Bot.SendTextMessageAsync(ChatId, text, replyMarkup: keyboard);
        }

        private async Task SendPic(string pic)
        {
            var fileName = $"Content/{pic}";

            if (File.Exists(fileName))
            {
                using (var stream = File.OpenRead(fileName))
                {
                    await Bot.SendPhotoAsync(ChatId, new InputOnlineFile(stream));
                }
            }
            else
            {
                log.Warn($"No pic {pic}");
            }
        }

    }
}