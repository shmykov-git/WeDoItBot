using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bot.Model.RoomPlaces
{
    public class ReplyButtonDialog : RoomPlace
    {
        public int ColumnsCount { get; set; } = 1;
        public string Caption { get; set; }
        public ReplyButton[] Buttons { get; set; }

        [JsonIgnore]
        public override IEnumerable<string> GoList => (Buttons?.Select(b => b.Go) ?? new string[0]).Concat(base.GoList);

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitReplyButtonDialog(this);
            await base.Visit(visitor);
        }
    }
}