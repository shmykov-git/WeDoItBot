using System.Threading.Tasks;
using Bot.Model.Artifacts;
using Bot.Model.Rooms;

namespace Bot.Model.RoomPlaces
{
    public class RoomPlace
    {
        public virtual Artifact[] Artifacts => new Artifact[0];
        public virtual async Task Visit(IBotMapVisitor visitor) { }
    }
}