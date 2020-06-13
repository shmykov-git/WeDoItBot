using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Suit.Extensions;
using TelegramBot.Model;
using TelegramBot.Tools;

namespace TelegramBot
{
	class TelegramBotSettings : IBotManagerSettings
    {
        private JObject settings;

        public TelegramBotSettings()
        {
            settings = File.ReadAllText("settings.json").FromJson<JObject>();
        }

        public SingleBotSettings[] Bots => settings["Bots"].Cast<JObject>().Select(bot => new SingleBotSettings()
        {
            Name = bot[nameof(SingleBotSettings.Name)].ToString(),
            BotMapFile = bot[nameof(SingleBotSettings.BotMapFile)].ToString(),
            BotToken = bot[nameof(SingleBotSettings.BotToken)].ToString(),
            ProxyHost = new Uri(bot[nameof(SingleBotSettings.ProxyHost)].ToString()),
        }).ToArray();
    }
}