using System;
using WeDoItBot.Tools;

namespace WeDoItBot
{
	class Settings : IBotStarterSettings
    {
        public string BotToken => "1107856285:AAHcDAuZg-BikqjCUAIb6ro8z3prriYR3sg";     // WeDoIt
        public Uri ProxyHost => new Uri("http://201.249.190.235:3128");
        public string BotFile => "bot.json";
    }
}