using System.Threading.Tasks;

namespace Bot.Model.Rooms
{
    public class GenPicRoom : ShowRoom
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }

        protected virtual void Simplify() { }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Simplify();

            await visitor.VisitGenRoom(this);
            await base.Visit(visitor);
        }
    }
}