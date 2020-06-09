using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TelegramBot.Tools
{
    class ContentManager
    {
        private Dictionary<string, byte[]> items = new Dictionary<string, byte[]>();
        private const string ContentFolder = "Content";

        public ContentManager()
        {
            LoadItems();
        }

        private void LoadItems()
        {
            foreach (var fileName in Directory.GetFiles(ContentFolder))
            {
                items.Add(Path.GetFileNameWithoutExtension(fileName).ToLower(), File.ReadAllBytes(fileName));
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