using Bot.Model.RoomPlaces;

namespace Bot.Model
{
    public class EnterItem
    {
        public string Key { get; set; }
        public EnterType Type { get; set; } = EnterType.Text;
        public string Name { get; set; }
    }
}