using Bot.Model.RoomPlaces;
using Suit.Extensions;

namespace Bot.Model.Rooms
{
    public class ShowRoom: Room
    {
        public RoomPlace[] Places { get; set; }

        public override void Visit(IBotMapVisitor visitor)
        {
            Places?.ForEach(p => p.Visit(visitor));

            base.Visit(visitor);
        }
    }
}