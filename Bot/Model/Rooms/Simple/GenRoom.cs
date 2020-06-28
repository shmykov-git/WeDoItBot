using System.Collections.Generic;
using System.Linq;
using Bot.Model.RoomPlaces;
using Newtonsoft.Json;

namespace Bot.Model.Rooms.Simple
{
    public class GenRoom : GenPicRoom
    {
        public string Gen { get; set; }
        public string Caption { get; set; }
        public string Go { get; set; }

        protected override void Simplify()
        {
            ActionName = Gen;

            Places = new RoomPlace[]
            {
                new Button()
                {
                    Caption = Caption,
                    Name = "Хорошо",
                    Go = Go
                },
            };
        }

        [JsonIgnore]
        public override IEnumerable<string> GoList => new[] { Go }.Concat(base.GoList);
    }
}