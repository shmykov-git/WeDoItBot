using System.Threading.Tasks;
using Bot.Model.Artifacts;
using Suit.Extensions;

namespace Bot.Model.Rooms
{
    public class Room
    {
        public string Key { get; set; }
        public string AutoGo { get; set; }

        public virtual Artifact[] Artifacts =>
            AutoGo.IsNullOrEmpty() ? new Artifact[0] : new[] {new Artifact() {Go = AutoGo}};

        public virtual async Task Visit(IBotMapVisitor visitor) => await visitor.VisitRoom(this);
    }
}