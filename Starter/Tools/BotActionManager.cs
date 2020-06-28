using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bot.PublicModel;
using Bot.PublicModel.ActionResult;
using TelegramBot.Tools;

namespace Starter.Tools
{
    class BotActionManager : IActionManagerSettings
    {
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

                default:
                    throw new NotImplementedException(arguments.ActionName);
            }
        }
    }
}
