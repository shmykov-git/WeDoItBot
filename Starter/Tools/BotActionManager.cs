using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bot.PublicModel;
using Bot.PublicModel.ActionResult;
using Suit.Logs;
using TelegramBot.Tools;

namespace Starter.Tools
{
    class BotActionManager : IActionManager
    {
        private readonly ILog log;
        private readonly IActionManagerSettings settings;

        public BotActionManager(ILog log, IActionManagerSettings settings)
        {
            this.log = log;
            this.settings = settings;
        }

        public async Task<ActionResult> DoAction(ActionArguments arguments)
        {
            switch (arguments.ActionName)
            {
                case "GenerateStatistics":
                    return new PicAndCaptionResult()
                    {
                        Caption = "Статистика посещения конференции",
                        Pic = File.ReadAllBytes("Content/Gen.png"),
                        NameGoes = new NameGo[]
                        {
                            ("В разработке", "underconstruction"),
                            ("Сервис", "service"),
                            ("Старт", "start"),
                            ("Хорошо", "service"),
                        },
                        ColumnsCount = 3
                    };

                case "VasyGenerated":
                    return new PicAndCaptionResult()
                    {
                        Caption = "Любая картинка и информация",
                        Pic = File.ReadAllBytes("Content/Gen.png"),
                        NameGoes = new NameGo[]
                        {
                            ("Ну ладно", "service"),
                        }
                    };

                case "GeneratePredictionThings":
                    return await GeneratePrediction("things", arguments);

                case "GeneratePredictionClothes":
                    return await GeneratePrediction("clothes", arguments);

                default:
                    throw new NotImplementedException(arguments.ActionName);
            }
        }

        public async Task<PicAndCaptionResult> GeneratePrediction(string modelName, ActionArguments arguments)
        {
            PicAndCaptionResult GetError() =>
                new PicAndCaptionResult()
                {
                    Pic = File.ReadAllBytes("Bots/Prediction/error.png"),
                    Caption = "Все сломалось, шеф...",
                };

            if (arguments.ActionOption == null)
                return GetError();

            try
            {
                byte[] bytes;
                using (var client = new HttpClient())
                {
                    bytes = await client.GetByteArrayAsync(arguments.ActionOption);
                }

                string prediction = null;
                using (var client = new HttpClient())
                {
                    var content = new ByteArrayContent(bytes);
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    var url = settings.GetPredictionApiUrl(modelName);
                    var result = await client.PostAsync(url, content);

                    if (result.IsSuccessStatusCode)
                    {
                        prediction = await result.Content.ReadAsStringAsync();
                    }
                }

                if (prediction == null)
                    GetError();

                return new PicAndCaptionResult()
                {
                    Caption = $"🌟🌟🌟 {prediction.ToUpper()} 🌟🌟🌟"
                };
            }
            catch (Exception e)
            {
                log.Exception(e);
                return GetError();
            }
        }
    }
}
