using System;
using TelegramBot.Tools;

namespace TelegramBot
{
	class TelegramBotSettings : IBotManagerSettings
    {
        public string BotToken => "1107856285:AAHcDAuZg-BikqjCUAIb6ro8z3prriYR3sg";     // WeDoIt
        public Uri ProxyHost => new Uri("http://201.249.190.235:3128");
        public string BotMapFile => "bot.json";
    }
}