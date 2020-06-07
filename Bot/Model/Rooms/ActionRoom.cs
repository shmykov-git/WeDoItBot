namespace Bot.Model.Rooms
{
    public class ActionRoom: Room
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }
        public string Go { get; set; }

        public override void Visit(IBotMapVisitor visitor)
        {
            visitor.DoAction(this);

            base.Visit(visitor);
        }
    }
}