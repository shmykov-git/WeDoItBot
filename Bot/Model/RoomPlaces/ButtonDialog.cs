using System.Linq;
using Bot.Model.Artifacts;

namespace Bot.Model.RoomPlaces
{
    public class ButtonDialog : RoomPlace
    {
        public string Question { get; set; }
        public Button[] Buttons { get; set; }

        public override Artifact[] Artifacts => Buttons.Select(b => new Artifact() { Go = b.Key }).ToArray();

        public override void Visit(IBotMapVisitor visitor)
        {
            visitor.VisitButtonDialog(this);

            base.Visit(visitor);
        }
    }
}