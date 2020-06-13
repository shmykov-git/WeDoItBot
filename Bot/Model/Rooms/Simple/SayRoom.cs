using System.Threading.Tasks;
using Bot.Model.RoomPlaces;

namespace Bot.Model.Rooms.Simple
{
    public class SayRoom : PicRoom
    {
        public string Say { get; set; }
        public string Go { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Places = new RoomPlace[]
            {
                new Button(){Go = Go, Name = "Хорошо", Caption = Say},
            };

            await base.Visit(visitor);
        }
    }
}