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

        protected override void Simplify()
        {
            Places = new RoomPlace[]
            {
                new Button()
                {
                    Caption = Say,
                    Name = "Хорошо",
                    Go = Go
                },
            };
        }

        [JsonIgnore]
        public override IEnumerable<string> GoList => new[] {Go}.Concat(base.GoList);
    }
}