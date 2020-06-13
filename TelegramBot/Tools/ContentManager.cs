using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Suit.Extensions;

namespace TelegramBot.Tools
{
    class ContentManager
    {
        private Dictionary<string, byte[]> items = new Dictionary<string, byte[]>();
        private const string ContentFolder = "Content";

        public ContentManager()
        {
            AddContentFolder(ContentFolder);
        }

        public void AddContentFolder(string contentFolder)
        {
            if (contentFolder.IsNullOrEmpty())
                return;

            foreach (var fileName in Directory.GetFiles(contentFolder))
            {
                var key = Path.GetFileNameWithoutExtension(fileName).ToLower();
                var bytes = File.ReadAllBytes(fileName);

                items.TryAdd(key, bytes);
            }
        }

        public async Task<bool> Apply(string contentKey, Func<Stream, Task> contentAction)
        {
            if (!items.TryGetValue(contentKey.ToLower(), out byte[] bytes))
                return false;

            using (var fileStream = new MemoryStream(bytes))
            {
                await contentAction(fileStream);
            }

            return true;
        }
    }
}