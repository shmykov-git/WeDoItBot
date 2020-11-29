using System.Threading.Tasks;
using Suit;
using Suit.Logs;
using TelegramBot;
using TelegramBot.Tools;

namespace Starter
{
    class Program
    {
        static Program()
        {
            IoC.Configure(IoCTelegramBot.Register, IoCStarter.Register);
        }

        static async Task Main(string[] args)
        {
            var log = IoC.Get<ILog>();

            log.Debug("### Start console ###");

            IoC.Get<ITelegramBotManager>().Start();

            while (true) await Task.Delay(1000);
        }
    }
}
