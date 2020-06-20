using System.Threading.Tasks;

namespace Bot.Model.Rooms
{
    public class ActionRoom: Room
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }

        protected virtual void Simplify() { }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Simplify();

            await visitor.VisitActionRoom(this);
            await base.Visit(visitor);
        }
    }
}