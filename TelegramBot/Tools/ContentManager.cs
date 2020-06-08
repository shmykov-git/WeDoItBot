using System;
using System.IO;

namespace TelegramBot.Tools
{
    class ContentManager
    {

        public void UseContent(Action<Stream> contentAction, string contentKey)
        {
            using (var fileStream = File.OpenRead($"Content/{contentKey}"))
            {
                contentAction(fileStream);
            }
        }
    }
}