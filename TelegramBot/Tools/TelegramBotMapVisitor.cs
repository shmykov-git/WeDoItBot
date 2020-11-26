using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bot.Extensions;
using Bot.Model;
using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;
using Bot.PublicModel;
using Bot.PublicModel.ActionResult;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Model;

namespace TelegramBot.Tools
{
    [LoggingAspect(LoggingRule.Input)]
    class TelegramBotMapVisitor: IBotMapVisitor
    {
        private readonly ILog log;
        private readonly ContentManager contentManager;
        private readonly ActionManager actionManager;
        private readonly TelegramUserContext context;

        private TelegramBotClient Client => context.Bot.Client;
        private long ChatId => context.Message.Chat.Id;

        public TelegramBotMapVisitor(ILog log, ContentManager contentManager, ActionManager actionManager, TelegramUserContext context)
        {
            this.log = log;
            this.contentManager = contentManager;
            this.actionManager = actionManager;
            this.context = context;

            contentManager.AddContentFolder(context.Bot.ContentFolder);
        }

        private async Task<bool> DoInternalAction(ActionRoom actionRoom)
        {
            switch (actionRoom.ActionName)
            {
                case "SendConfig":
                    await SendText(context.Bot.BotConfig.Cut(1024));
                    await SendFile(context.Bot.BotMapFile);
                    return true;
            }

            return false;
        }

        public async Task VisitActionRoom(ActionRoom actionRoom)
        {
            if (actionRoom.ActionName == null)
                return;

            if (await DoInternalAction(actionRoom))
                return;

            throw new NotImplementedException(actionRoom.ToJsonStr());
        }

        public Task VisitRoom(Room room)
        {
            return Task.CompletedTask;
        }

        public Task VisitActionRoomPlace(ActionRoomPlace actionRoomPlace)
        {
            return Task.CompletedTask;
        }

        public async Task VisitButton(Button button)
        {
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
            await SendText(enterPlace.Name);
        }

        public async Task VisitGenRoom(GenPicRoom genPicRoom)
        {
            string value = null;
            if (genPicRoom.ActionArgument != null)
                context.State.Values.TryGetValue(genPicRoom.ActionArgument, out value);

            var result = await actionManager.DoAction(new ActionArguments()
            {
                BotName = context.Bot.Name,
                ActionName = genPicRoom.ActionName,
                ActionOption = value
            });

            if (result is PicAndCaptionResult picAndCaptionResult)
            {
                if (picAndCaptionResult.Pic != null)
                    await SendPic(picAndCaptionResult.Pic);

                genPicRoom.Generate(picAndCaptionResult);

                return;
            }

            throw new NotImplementedException(genPicRoom.ToJsonStr());
        }

        public async Task VisitPicRoom(PicRoom picRoom)
        {
            await SendText(picRoom.Name);
            await SendPic(picRoom.Pic);
            await SendText(picRoom.Caption);
        }


        private async Task SendText(string text)
        {
            if (text.IsNullOrEmpty())
                return;

            if (text.Length > 4096)
                text = text.Substring(0, 4096);

            await Client.SendTextMessageAsync(ChatId, text);
        }

        private async Task SendFile(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            using (var stream = File.OpenRead(fileName))
            {
                await Client.SendDocumentAsync(ChatId, new InputOnlineFile(stream), Path.GetFileName(fileName));
            }
        }

        private async Task SendDialog(string text, IReplyMarkup replyMarkup)
        {
            await Client.SendTextMessageAsync(ChatId, text, replyMarkup: replyMarkup);
        }

        private async Task SendPic(string pic)
        {
            if (context.Bot.Settings.UsePicRefs)
            {
                if (!await contentManager.ApplyRef(pic, async picRef => await Client.SendPhotoAsync(ChatId, new InputOnlineFile(picRef))))
                    log.Warn($"No pic {pic}");
            }
            else
            {
                if (!await contentManager.ApplyImgData(pic, async stream => await Client.SendPhotoAsync(ChatId, new InputOnlineFile(stream))))
                    log.Warn($"No pic {pic}");
            }
        }

        private async Task SendPic(byte[] pic)
        {
            using (var fileStream = new MemoryStream(pic))
            {
                await Client.SendPhotoAsync(ChatId, new InputOnlineFile(fileStream));
            }
        }

    }
}