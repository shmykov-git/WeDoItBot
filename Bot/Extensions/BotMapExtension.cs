using System.Linq;
using Bot.Model;
using Bot.Model.Rooms;

namespace Bot.Extensions
{
    public static class BotMapExtension
    {
        public static Room FindRoom(this BotMap map, string key)
        {
            return map.Rooms.FirstOrDefault(r => r.Key == key);
        }
    }
}
