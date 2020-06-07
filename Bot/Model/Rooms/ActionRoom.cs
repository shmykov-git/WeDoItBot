namespace Bot.Model.Rooms
{
    public class ActionRoom: Room
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }

        public override void Visit(IBotMapVisitor visitor)
        {
            visitor.VisitActionRoom(this);

            base.Visit(visitor);
        }
    }
}