using System;
using TelegramBot.Model;

namespace TelegramBot.Tools
{
    public interface ITelegramBotManagerSettings
    {
        SingleBotSettings[] Bots { get; }
    }
}