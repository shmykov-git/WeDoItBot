namespace Bot.Model.Rooms.Simple
{
    public class AutoGoRoom : ActionRoom
    {
        public string Do { get; set; }

        protected override void Simplify()
        {
            ActionName = Do;
        }
    }
}