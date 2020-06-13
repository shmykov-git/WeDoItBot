using System;
using TelegramBot.Model;

namespace TelegramBot.Tools
{
    interface IBotManagerSettings
    {
        SingleBotSettings[] Bots { get; }
    }
}