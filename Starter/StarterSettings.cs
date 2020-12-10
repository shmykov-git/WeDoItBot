using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Suit.Extensions;
using TelegramBot.Model;
using TelegramBot.Tools;

namespace Starter
{
	class StarterSettings : ITelegramBotManagerSettings, IActionManagerSettings
    {
        private JObject settings;

        public StarterSettings()
        {
            settings = File.ReadAllText("settings.json").FromJson<JObject>();
        }

        public SingleBotSettings[] Bots => settings["Bots"].Cast<JObject>().Select(bot => bot.ToObject<SingleBotSettings>()).ToArray();
        public string BotFiles => settings[nameof(BotFiles)]?.ToString();

        public string GetBotFile(string token, string path) =>
            BotFiles.Replace("<token>", token).Replace("<file_path>", path);

        public string PredictionApiUrl => Environment.GetEnvironmentVariable("PREDICTION_APIURL") ?? "http://localhost:5000/predict/<model_name>";

        public string GetPredictionApiUrl(string modelName) => PredictionApiUrl.Replace("<model_name>", modelName);

    }
}