using System;
using TelegramBot.Model;
using TelegramBot.Tools;

namespace TelegramBot
{
	class TelegramBotSettings : IBotManagerSettings
    {
        public SingleBotSettings[] Bots => new[]
        {
            new SingleBotSettings()
            {
                Name = "@WeDoItHakatonBot",
                BotMapFile = @"C:\Projects\WeDoItBot\Bot\Bots\bot.json",
                BotToken = "1107856285:AAHcDAuZg-BikqjCUAIb6ro8z3prriYR3sg",
                ProxyHost = new Uri("http://201.249.190.235:3128")
            },
            new SingleBotSettings()
            {
                Name = "@vasy_do_it_bot",
                BotMapFile = @"C:\Projects\WeDoItBot\Bot\Bots\bot.json",
                BotToken = "1245398123:AAEz5SV8Jgt0L8NaEXj-3yWddqOWesNVaIk",
                ProxyHost = new Uri("http://201.249.190.235:3128")
            },
        };
    }
}