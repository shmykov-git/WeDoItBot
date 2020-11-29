using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bot.Model;
using Suit.Extensions;

namespace TelegramBot.Tools
{
    class ContentManager
    {
        private Dictionary<string, byte[]> dataItems = new Dictionary<string, byte[]>();
        private Dictionary<string, string> refItems = new Dictionary<string, string>();

        private const string ContentFolder = "Content";

        public ContentManager()
        {
            AddContentFolder(ContentFolder);
        }

        public void AddContentFolder(string contentFolder)
        {
            if (contentFolder.IsNullOrEmpty())
                return;

            if (!Directory.Exists(contentFolder))
                return;

            foreach (var fileName in Directory.GetFiles(contentFolder))
            {
                if (Path.GetFileName(fileName) == "mapper.json")
                {
                    AddRefs(fileName);

                    continue;
                }

                var key = Path.GetFileNameWithoutExtension(fileName).ToLower();
                var bytes = File.ReadAllBytes(fileName);

                dataItems.Add(key, bytes);
            }
        }

        private void AddRefs(string fileName)
        {
            var mapper = File.ReadAllText(fileName).FromJson<PicMapper>();
            mapper.Pics.ForEach(pic => refItems.Add(pic.Key.ToLower(), pic.Pic));
        }

        public async Task<bool> ApplyRef(string contentKey, Func<string, Task> contentAction)
        {
            if (!refItems.TryGetValue(contentKey.ToLower(), out string picRef))
                return false;

            await contentAction(picRef);

            return true;
        }

        public async Task<bool> ApplyImgData(string contentKey, Func<Stream, Task> contentAction)
        {
            if (!dataItems.TryGetValue(contentKey.ToLower(), out byte[] bytes))
                return false;

            using (var fileStream = new MemoryStream(bytes))
            {
                await contentAction(fileStream);
            }

            return true;
        }
    }
}