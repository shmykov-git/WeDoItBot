using System.Linq;
using Bot.Model;
using Bot.Model.Rooms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suit.Extensions;

namespace Bot.Extensions
{
    public static class BotMapExtensions
    {
        public static BotMap ToBotMap(this string jsonStr)
        {
            var json = jsonStr.FromJson<JObject>();

            foreach (JObject jRoom in json["Rooms"])
            {
                if (jRoom.ContainsKey("Type"))
                    jRoom.AddFirst(new JProperty("$type", $"Bot.Model.Rooms.Simple.{jRoom["Type"]}Room, Bot"));
            }

            return json.ToObject<BotMap>(new JsonSerializer() {TypeNameHandling = TypeNameHandling.All});
        }

        public static Room FindRoom(this BotMap map, string key)
        {
            return map.Rooms.First(r => r.Key == key);
        }
    }
}
