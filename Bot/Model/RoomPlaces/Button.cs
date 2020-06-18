using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bot.Model.RoomPlaces
{
    public class Button : RoomPlace
    {
        public string Caption { get; set; }
        public string Go { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public override IEnumerable<string> GoList => new[] { Go }.Concat(base.GoList);

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitButton(this);
            await base.Visit(visitor);
        }
    }
}