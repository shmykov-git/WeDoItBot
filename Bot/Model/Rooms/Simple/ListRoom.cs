using System.Linq;
using System.Threading.Tasks;
using Bot.Model.RoomPlaces;

namespace Bot.Model.Rooms.Simple
{
    public class ListRoom : PicRoom
    {
        public string List { get; set; }
        public ListItem[] Items { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Places = new RoomPlace[]
            {
                new ButtonDialog()
                {
                    Caption = List,
                    Buttons = Items.Select(item => new Button() {Name = item.Button, Go = item.Go}).ToArray(),
                },
            };

            await base.Visit(visitor);
        }
    }
}