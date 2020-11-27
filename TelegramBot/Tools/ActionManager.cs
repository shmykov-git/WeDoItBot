using System.Threading.Tasks;
using Bot.PublicModel;
using Bot.PublicModel.ActionResult;
using Suit.Aspects;
using Suit.Logs;
using TelegramBot.Model;

namespace TelegramBot.Tools
{
    [LoggingAspect(LoggingRule.Input)]
    class ActionManager
    {
        private readonly ILog log;
        private readonly IActionManager settings;

        public ActionManager(ILog log, IActionManager settings)
        {
            this.log = log;
            this.settings = settings;
        }

        //TODO: api integration POST(https://anyapi.com/api/{ActionName}, BODY = ActionArgument)
        //TODO: process and show result
        //TODO: move it to separate dll for indepandant usage
        public Task<ActionResult> DoAction(ActionArguments arguments)
        {
            return settings.DoAction(arguments);
        }
    }
}