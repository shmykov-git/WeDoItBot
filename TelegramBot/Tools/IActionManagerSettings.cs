using System.Threading.Tasks;
using Bot.PublicModel;
using Bot.PublicModel.ActionResult;
using TelegramBot.Model;

namespace TelegramBot.Tools
{
    public interface IActionManagerSettings
    {
        Task<ActionResult> DoAction(ActionArguments arguments);
    }
}