using System.Linq;
using System.Threading.Tasks;
using Bot.Model.RoomPlaces;

namespace Bot.Model.Rooms.Simple
{
    public class EnterRoom : PicRoom
    {
        public string Enter { get; set; }
        public EnterItem[] Items { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Caption = Enter;

            Places = Places ?? Items.Select(item => new EnterPlace()
            {
                Key = item.Key,
                Name = item.Name,
            }).ToArray();

            await base.Visit(visitor);
        }
    }
}