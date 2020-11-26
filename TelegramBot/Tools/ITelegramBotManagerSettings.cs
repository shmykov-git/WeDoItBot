using System;
using TelegramBot.Model;

namespace TelegramBot.Tools
{
    public interface ITelegramBotManagerSettings
    {
        SingleBotSettings[] Bots { get; }
        string BotFiles { get; }
        string GetBotFile(string token, string path);
    }
}