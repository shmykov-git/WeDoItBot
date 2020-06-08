using System;
using System.Collections.Concurrent;
using System.Net;
using Bot.Model;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace TelegramBot.Tools
{
    [LoggingAspect(LoggingRule.Full)]
    class TelegramBotManager
    {
        private readonly ILog log;
        private readonly IBotManagerSettings settings;
        private readonly Func<TelegramUserContext> createUserContextFn;
        private BotMap botMap;

        public TelegramBotClient Bot { get; private set; }

        private ConcurrentDictionary<string, TelegramUserContext> contexts = new ConcurrentDictionary<string, TelegramUserContext>();
        private string GetUserKey(Message message) => message.Chat.Username;

        public TelegramBotManager(ILog log, IBotManagerSettings settings, Func<TelegramUserContext> createUserContextFn)
        {
            this.log = log;
            this.settings = settings;
            this.createUserContextFn = createUserContextFn;

            botMap = File.ReadAllText(settings.BotMapFile).FromNamedJson<BotMap>();
        }

        private TelegramUserContext GetContext(CallbackQuery query)
        {
            var context = GetContext(query.Message);
            context.CallbackQuery = query;

            return context;
        }

        private TelegramUserContext GetContext(Message message)
        {
            return contexts.AddOrUpdate(GetUserKey(message),
                userKey =>
                {
                    var newContext = createUserContextFn();
                    newContext.UserKey = userKey;
                    newContext.Message = message;
                    newContext.Map = botMap;
                    newContext.State = new State();

                    return newContext;

                },
                (k, oldContext) =>
                {
                    oldContext.Message = message;
                    oldContext.CallbackQuery = null;

                    return oldContext;
                });
        }

        public void Start()
        {
            var proxy = new WebProxy(settings.ProxyHost);
            Bot = new TelegramBotClient(settings.BotToken, proxy);

            Bot.OnMessage += (o, a) =>
            {
                try
                {
                    if (IsActual(a.Message))
                        GetContext(a.Message).Maestro.Type(a.Message.Text);
                }
                catch (Exception e)
                {
                    log.Exception(e);
                }
            };

            Bot.OnCallbackQuery += (o, a) =>
            {
                try
                {
                    if (IsActual(a.CallbackQuery.Message))
                        GetContext(a.CallbackQuery).Maestro.Command(a.CallbackQuery.Data);
                }
                catch (Exception e)
                {
                    log.Exception(e);
                }
            };

            Bot.OnReceiveGeneralError += (o, a) => log.Exception(a.Exception);
            Bot.OnReceiveError += (o, a) => log.Exception(a.ApiRequestException);

            Bot.StartReceiving();
            log.Info($"Start listening");

            Console.ReadLine();
            Bot.StopReceiving();
        }

        private bool IsActual(Message message) => message.Date.AddMinutes(10) > DateTime.UtcNow;

    }
}
