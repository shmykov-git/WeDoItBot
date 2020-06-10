using System.Threading.Tasks;

namespace Bot.Model.RoomPlaces
{
    public class EnterPlace : RoomPlace
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.EnterPlace(this);
            await base.Visit(visitor);
        }
    }
}