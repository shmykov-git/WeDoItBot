using System.Collections.Concurrent;
using Bot.Model;
using Suit.Logs;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Model;

namespace TelegramBot.Tools
{
    class TelegramUserContext
    {
        private readonly ILog log;

        public IBotMapVisitor Visitor { get; set; }
        public IBotMaestro Maestro { get; set; }

        public ContentManager ContentManager { get; set; }

        public string UserKey { get; set; }
        public SingleBot Bot { get; set; }
        public Message Message { get; set; }
        public CallbackQuery CallbackQuery { get; set; }
        public ConcurrentDictionary<string, TelegramUserContext> Contexts { get; set; }
        public State State { get; set; }

        public TelegramUserContext(ILog log)
        {
            this.log = log;
        }
    }
}