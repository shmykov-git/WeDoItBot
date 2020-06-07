namespace Bot.Model.Rooms
{
    public class Room
    {
        public string Key { get; set; }

        public virtual void Visit(IBotMapVisitor visitor) { }
    }
}