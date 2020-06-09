using System.Threading.Tasks;
using Bot.Model.RoomPlaces;

namespace Bot.Model.Rooms.Simple
{
    public class SayRoom : PicRoom
    {
        public string Caption { get; set; }
        public string Go { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Places = new RoomPlace[]
            {
                new Button(){Key = Go, Name = "Хорошо", Caption = Caption},
            };

            await base.Visit(visitor);
        }
    }
}