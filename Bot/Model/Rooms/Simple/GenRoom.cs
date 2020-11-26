using System.Collections.Generic;
using System.Linq;
using Bot.Model.RoomPlaces;
using Newtonsoft.Json;

namespace Bot.Model.Rooms.Simple
{
    public class GenRoom : GenPicRoom
    {
        public string Gen { get; set; }
        public string GenKey { get; set; }

        protected override void Simplify()
        {
            ActionName = Gen;
            ActionArgument = GenKey;
        }

        [JsonIgnore]
        public override IEnumerable<string> GoList => new[] { Go }.Concat(base.GoList);
    }
}