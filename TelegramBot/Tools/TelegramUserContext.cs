using Bot.Model;
using Suit.Logs;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Tools
{
    class TelegramUserContext
    {
        private readonly ILog log;

        public TelegramBotClient Bot { get; set; }
        public IBotMapVisitor Visitor { get; set; }
        public IBotMaestro Maestro { get; set; }

        public string UserKey { get; set; }
        public BotMap Map { get; set; }
        public Message Message { get; set; }
        public CallbackQuery CallbackQuery { get; set; }
        public State State { get; set; }

        public TelegramUserContext(ILog log)
        {
            this.log = log;
        }
    }
}