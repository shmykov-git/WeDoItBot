using System.Threading.Tasks;

namespace Bot.Model.RoomPlaces
{
    public class ActionRoomPlace : RoomPlace
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitActionRoomPlace(this);
            await base.Visit(visitor);
        }
    }
}