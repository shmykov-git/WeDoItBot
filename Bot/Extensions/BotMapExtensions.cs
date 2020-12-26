using System.Linq;
using Bot.Model;
using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;
using Bot.Model.Rooms.Simple;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suit.Extensions;

namespace Bot.Extensions
{
    public static class BotMapExtensions
    {
        //todo: reflection, order
        private static string[] roomTypes = {"Ask", "List", "Menu", "Say", "Enter", "AutoGo", "Gen" };

        public static BotMap ToBotMap(this string jsonStr)
        {
            var json = jsonStr.FromJson<JObject>();

            foreach (JObject jRoom in json["Rooms"])
            {
                if (jRoom.ContainsKey("Type"))
                    jRoom.AddFirst(new JProperty("$type", $"Bot.Model.Rooms.Simple.{jRoom["Type"]}Room, Bot"));

                foreach (var roomType in roomTypes)
                {
                    if (jRoom.ContainsKey(roomType))
                    {
                        jRoom.AddFirst(new JProperty("$type", $"Bot.Model.Rooms.Simple.{roomType}Room, Bot"));
                        break;
                    }
                }
            }

            return json.ToObject<BotMap>(new JsonSerializer() {TypeNameHandling = TypeNameHandling.All});
        }

        public static bool RoomExists(this BotMap map, string key)
        {
            return map.Rooms.Any(r => r.Key == key);
        }
        public static Room FindRoom(this BotMap map, string key)
        {
            return map.Rooms.First(r => r.Key == key);
        }

        public static string FindReplyGo(this BotMap map, string key)
        {
            return map.Rooms.OfType<MenuRoom>().SelectMany(mr=>mr.Items).FirstOrDefault(b=>b.Button == key)?.Go;
        }
    }
}
