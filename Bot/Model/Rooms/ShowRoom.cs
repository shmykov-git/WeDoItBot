using System.Linq;
using System.Threading.Tasks;
using Bot.Model.Artifacts;
using Bot.Model.RoomPlaces;
using Suit.Extensions;

namespace Bot.Model.Rooms
{
    public class ShowRoom: Room
    {
        public RoomPlace[] Places { get; set; }

        public override Artifact[] Artifacts => Places.SelectMany(p => p.Artifacts).ToArray();

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Places?.ForEach(async p => await p.Visit(visitor));

            await base.Visit(visitor);
        }
    }
}