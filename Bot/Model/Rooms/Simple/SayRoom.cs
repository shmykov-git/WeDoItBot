using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Model.RoomPlaces;
using Newtonsoft.Json;

namespace Bot.Model.Rooms.Simple
{
    public class SayRoom : PicRoom
    {
        public string Say { get; set; }
        public string Go { get; set; }

        [JsonIgnore]
        public override IEnumerable<string> GoList => new[] {Go}.Concat(base.GoList);

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