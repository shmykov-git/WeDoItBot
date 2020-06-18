using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bot.Model.RoomPlaces
{
    public class ButtonMenu : RoomPlace
    {
        public int ColumnsCount { get; set; } = 1;
        public string Caption { get; set; }
        public Button[] Buttons { get; set; }

        [JsonIgnore]
        public override IEnumerable<string> GoList => Buttons.Select(b => b.Go).Concat(base.GoList);

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitButtonMenu(this);
            await base.Visit(visitor);
        }

    }
}