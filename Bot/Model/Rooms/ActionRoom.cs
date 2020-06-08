using System.Threading.Tasks;

namespace Bot.Model.Rooms
{
    public class ActionRoom: Room
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitActionRoom(this);
            await base.Visit(visitor);
        }
    }
}