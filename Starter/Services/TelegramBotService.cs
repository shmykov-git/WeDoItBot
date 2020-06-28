using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Suit.Logs;
using TelegramBot.Tools;

namespace TelegramBot.Services
{
    class TelegramBotService : IHostedService
    {
        private readonly ILog log;
        private readonly ITelegramBotServiceSettings settings;
        private readonly ITelegramBotManager botManager;

        public TelegramBotService(ILog log, ITelegramBotServiceSettings settings, ITelegramBotManager botManager)
        {
            this.log = log;
            this.settings = settings;
            this.botManager = botManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                log.Debug("### Start service ###");

                botManager.Start();
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                log.Debug("### Stop service ###");

                botManager.Stop();
            }, cancellationToken);
        }
    }
}
