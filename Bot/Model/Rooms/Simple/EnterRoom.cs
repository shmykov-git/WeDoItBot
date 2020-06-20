using System.Linq;
using Bot.Model.RoomPlaces;

namespace Bot.Model.Rooms.Simple
{
    public class EnterRoom : PicRoom
    {
        public string Enter { get; set; }
        public EnterItem[] Items { get; set; }

        protected override void Simplify()
        {
            Caption = Enter;

            if (Places == null)
                Places = Items.Select(item => new EnterPlace()
                {
                    Key = item.Key,
                    Name = item.Name
                }).ToArray();
        }
    }
}