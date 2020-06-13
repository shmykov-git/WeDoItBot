using Bot.Model;
using Telegram.Bot;

namespace TelegramBot.Model
{
    class SingleBot
    {
        public string Name { get; set; }
        public string BotConfig { get; set; }
        public BotMap Map { get; set; }
        public string ContentFolder { get; set; }
        public TelegramBotClient Client { get; set; }
    }
}