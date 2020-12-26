using System.Linq;
using Bot.Model.Rooms.Simple;

namespace Bot.Extensions
{
    public static class MenuRoomExtentions
    {
        public static string FindReplyGo(this MenuRoom room, string key)
        {
            return room.Items.FirstOrDefault(b => b.Button == key)?.Go;
        }
    }
}