namespace Bot.Model.RoomPlaces
{
    public class ActionRoomPlace : RoomPlace
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }

        public override void Visit(IBotMapVisitor visitor)
        {
            visitor.VisitActionRoomPlace(this);

            base.Visit(visitor);
        }
    }
}