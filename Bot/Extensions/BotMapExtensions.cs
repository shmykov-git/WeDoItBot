using System.Linq;
using Bot.Model;
using Bot.Model.Rooms;

namespace Bot.Extensions
{
    public static class BotMapExtensions
    {
        public static Room FindRoom(this BotMap map, string key)
        {
            return map.Rooms.First(r => r.Key == key);
        }
    }
}
