using System.Threading.Tasks;

namespace Bot.Model.Rooms
{
    public class PicRoom : ShowRoom
    {
        private string pic;
        public string Name { get; set; }

        public string Pic
        {
            get => pic ?? Id;
            set => pic = value;
        }

        public string Caption { get; set; }

        protected virtual void Simplify() { }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Simplify();

            if (EnterPlace == null)
                await visitor.VisitPicRoom(this);

            await base.Visit(visitor);
        }
    }
}