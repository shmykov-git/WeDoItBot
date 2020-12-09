﻿using System;
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
                
                case "GeneratePrediction":
                    return await GeneratePrediction(arguments);

                default:
                    throw new NotImplementedException(arguments.ActionName);
            }
        }

        public async Task<PicAndCaptionResult> GeneratePrediction(ActionArguments arguments)
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
                    client.BaseAddress = new Uri(settings.PredictionApiUrl);

                    var content = new ByteArrayContent(bytes);
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    var result = await client.PostAsync("predict", content);

                    if (result.IsSuccessStatusCode)
                    {
                        prediction = await result.Content.ReadAsStringAsync();
                    }
                }

                if (prediction == null)
                    GetError();

                return new PicAndCaptionResult()
                {
                    Caption = $"Это {prediction}. Попробовать еще раз?",
                    NameGoes = new NameGo[]
                    {
                        ("Да", "things"),
                        ("Нет", "service")
                    },
                    ColumnsCount = 2
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
