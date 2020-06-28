namespace Bot.PublicModel.ActionResult
{
    public class NameGo
    {
        public string Name { get; set; }
        public string Go { get; set; }

        public static implicit operator NameGo((string, string) v)
        {
            return new NameGo() {Name = v.Item1, Go = v.Item2};
        }
    }
}