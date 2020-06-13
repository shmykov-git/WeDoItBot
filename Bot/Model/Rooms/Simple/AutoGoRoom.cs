using System.Linq;
using System.Threading.Tasks;
using Bot.Model.RoomPlaces;

namespace Bot.Model.Rooms.Simple
{
    public class AutoGoRoom : ActionRoom
    {
        public string Do { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            ActionName = Do;

            await base.Visit(visitor);
        }
    }
}