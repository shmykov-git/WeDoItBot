using System.Threading.Tasks;
using Bot.Model.Artifacts;

namespace Bot.Model.RoomPlaces
{
    public class Button : RoomPlace
    {
        public string Caption { get; set; }
        public string Go { get; set; }
        public string Name { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitButton(this);
            await base.Visit(visitor);
        }
    }
}