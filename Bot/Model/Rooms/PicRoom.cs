using System.Threading.Tasks;

namespace Bot.Model.Rooms
{
    public class PicRoom : ShowRoom
    {
        public string Name { get; set; }
        public string Pic { get; set; }
        public string Description { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitPicRoom(this);

            await base.Visit(visitor);
        }
    }
}