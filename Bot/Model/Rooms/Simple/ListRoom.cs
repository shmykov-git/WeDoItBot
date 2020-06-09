using System.Threading.Tasks;
using Bot.Model.RoomPlaces;

namespace Bot.Model.Rooms.Simple
{
    public class ListRoom : PicRoom
    {
        public string Caption { get; set; }
        public Button[] Buttons { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Places = new RoomPlace[]
            {
                new ButtonDialog()
                {
                    Caption = Caption,
                    Buttons = Buttons
                },
            };

            await base.Visit(visitor);
        }
    }
}