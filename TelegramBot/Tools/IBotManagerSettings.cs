using System;

namespace TelegramBot.Tools
{
    interface IBotManagerSettings
    {
        string BotMapFile { get; }
        Uri ProxyHost { get; }
        string BotToken { get; }
    }
}