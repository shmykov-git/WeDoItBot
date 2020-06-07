using System;
using Suit;
using Suit.Logs;
using TelegramBot.Tools;

namespace TelegramBot
{
    class Program
    {
        static Program()
        {
            IoC.Configure(IoCTelegramBot.Register);
        }

        static void Main(string[] args)
        {
            var log = IoC.Get<ILog>();
            log.Debug("### Start ###");

            IoC.Get<TelegramBotManager>().Start();

            log.Debug("### End ###");
        }
    }
}
