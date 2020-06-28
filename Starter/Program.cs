using System;
using Starter;
using Suit;
using Suit.Logs;
using TelegramBot;
using TelegramBot.Tools;

namespace WeDoItStarter
{
    class Program
    {
        static Program()
        {
            IoC.Configure(IoCTelegramBot.Register, IoCStarter.Register);
        }

        static void Main(string[] args)
        {
            var log = IoC.Get<ILog>();
            //var isService = args.Length > 0 && args[0] == "service";

            //var builder = new HostBuilder()
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        services.AddHostedService<TelegramBotService>();
            //    });

            //if (isService)
            //{
            //    await builder.RunAsServiceAsync();
            //}
            //else
            //{
            //    await builder.RunConsoleAsync();
            //}

            //if (args.Length > 0 && args[0] == "service")
            //{
            //    ServiceBase.Run(IoC.Get<TelegramBotService>());
            //}
            //else
            //{
                log.Debug("### Start console ###");

                IoC.Get<ITelegramBotManager>().Start();

                Console.ReadLine();

                log.Debug("### End ###");
            //}
        }
    }
}
