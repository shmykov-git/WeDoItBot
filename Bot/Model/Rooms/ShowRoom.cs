using System.Linq;
using Bot.Model.Artifacts;
using Bot.Model.RoomPlaces;
using Suit.Extensions;

namespace Bot.Model.Rooms
{
    public class ShowRoom: Room
    {
        public RoomPlace[] Places { get; set; }

        public override Artifact[] Artifacts => Places.SelectMany(p => p.Artifacts).ToArray();

        public override void Visit(IBotMapVisitor visitor)
        {
            Places?.ForEach(p => p.Visit(visitor));

            base.Visit(visitor);
        }
    }
}