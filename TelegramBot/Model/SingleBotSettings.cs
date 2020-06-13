using System;

namespace TelegramBot.Model
{
    class SingleBotSettings
    {
        public string Name { get; set; }
        public string BotMapFile { get; set; }
        public Uri ProxyHost { get; set; }
        public string BotToken { get; set; }
    }
}