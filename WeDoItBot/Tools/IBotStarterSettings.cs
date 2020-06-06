using System;

namespace WeDoItBot.Tools
{
    interface IBotStarterSettings
    {
        string BotToken { get; }
        Uri ProxyHost { get; }
        string BotFile { get; }
    }
}