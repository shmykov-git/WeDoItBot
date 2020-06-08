using System.Linq;
using System.Threading.Tasks;
using Bot.Model.Artifacts;

namespace Bot.Model.RoomPlaces
{
    public class ButtonDialog : RoomPlace
    {
        public string Question { get; set; }
        public Button[] Buttons { get; set; }

        public override Artifact[] Artifacts => Buttons.Select(b => new Artifact() { Go = b.Key }).ToArray();

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitButtonDialog(this);
            await base.Visit(visitor);
        }
    }
}