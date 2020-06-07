using System.Linq;

namespace Bot.Model.RoomPlaces
{
    public class ButtonDialog : RoomPlace
    {
        public string Question { get; set; }
        public Button[] Buttons { get; set; }

        public override void Visit(IBotMapVisitor visitor)
        {
            visitor.ShowButtonDialog(this);

            base.Visit(visitor);
        }
    }
}