using System.Linq;
using System.Threading.Tasks;
using Bot.Model.Artifacts;

namespace Bot.Model.RoomPlaces
{
    public class ButtonDialog : RoomPlace
    {
        public int ColumnsCount { get; set; } = 1;
        public string Caption { get; set; }
        public Button[] Buttons { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitButtonDialog(this);
            await base.Visit(visitor);
        }
    }
}