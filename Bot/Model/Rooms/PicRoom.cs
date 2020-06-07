namespace Bot.Model.Rooms
{
    public class PicRoom : ShowRoom
    {
        public string Name { get; set; }
        public string Pic { get; set; }
        public string Description { get; set; }

        public override void Visit(IBotMapVisitor visitor)
        {
            visitor.ShowPicRoom(this);

            base.Visit(visitor);
        }
    }
}