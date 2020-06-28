using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using Bot.Extensions;
using Bot.Model;
using Suit.Aspects;
using Suit.Extensions;
using Suit.Logs;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Model;
using File = System.IO.File;

namespace TelegramBot.Tools
{
    [LoggingAspect(LoggingRule.Stabilize)]
    class TelegramBotManager : ITelegramBotManager
    {
        private readonly ILog log;
        private readonly ITelegramBotManagerSettings settings;
        private readonly Func<SingleBot, TelegramUserContext> createUserContextFn;

        public SingleBot[] Bots { get; set; }

        private ConcurrentDictionary<string, TelegramUserContext> contexts = new ConcurrentDictionary<string, TelegramUserContext>();
        private string GetUserKey(SingleBot bot, Message message) => $"{message.Chat.Username}{bot.Name}";

        public TelegramBotManager(ILog log, ITelegramBotManagerSettings settings, Func<SingleBot, TelegramUserContext> createUserContextFn)
        {
            this.log = log;
            this.settings = settings;
            this.createUserContextFn = createUserContextFn;
        }

        private TelegramUserContext GetContext(SingleBot bot, CallbackQuery query)
        {
            var context = GetContext(bot, query.Message);
            context.CallbackQuery = query;

            return context;
        }

        private TelegramUserContext GetContext(SingleBot bot, Message message)
        {
            var contextKey = GetUserKey(bot, message);

            return contexts.AddOrUpdate(contextKey,
                userKey =>
                {
                    var newContext = createUserContextFn(bot);
                    newContext.UserKey = userKey;
                    newContext.Message = message;
                    newContext.Contexts = contexts;
                    newContext.State = new State();

                    log.Info($"UserContext {contextKey} created");

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
            Bots = settings.Bots.Select(botSettings =>
            {
                var bot = new SingleBot()
                {
                    Name = botSettings.Name,
                    ContentFolder = botSettings.Content,
                };

                bot.Settings = botSettings;
                bot.BotMapFile = botSettings.BotMapFile;
                bot.BotConfig = File.ReadAllText(botSettings.BotMapFile);
                bot.Map = bot.BotConfig.ToBotMap();

                if (botSettings.ProxyHost.IsNullOrEmpty())
                {
                    bot.Client = new TelegramBotClient(botSettings.BotToken);
                }
                else
                {
                    var proxy = new WebProxy(botSettings.ProxyHost);
                    bot.Client = new TelegramBotClient(botSettings.BotToken, proxy);
                }

                bot.Client.OnMessage += (o, a) =>
                {
                    try
                    {
                        if (IsActual(a.Message))
                            GetContext(bot, a.Message).Maestro.Type(a.Message.Text);
                    }
                    catch (Exception e)
                    {
                        log.Exception(e);
                    }
                };

                bot.Client.OnCallbackQuery += (o, a) =>
                {
                    try
                    {
                        if (IsActual(a.CallbackQuery.Message))
                            GetContext(bot, a.CallbackQuery).Maestro.Command(a.CallbackQuery.Data);
                    }
                    catch (Exception e)
                    {
                        log.Exception(e);
                    }
                };

                bot.Client.OnReceiveGeneralError += (o, a) => log.Exception(a.Exception);
                bot.Client.OnReceiveError += (o, a) => log.Exception(a.ApiRequestException);

                bot.Client.StartReceiving();

                log.Info($"Start listening bot {botSettings.Name}");

                return bot;
            }).ToArray();
        }

        public void Stop()
        {
            Bots.ForEach(b => b.Client.StopReceiving());
        }

        private bool IsActual(Message message) => true; //message.Date.AddMinutes(10) > DateTime.UtcNow;
    }
}
