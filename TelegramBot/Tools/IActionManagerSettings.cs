namespace TelegramBot.Tools
{
    public interface IActionManagerSettings
    {
        string GetPredictionApiUrl(string modelName);
    }
}