namespace Bot.PublicModel.ActionResult
{
    public class PicAndCaptionResult : ActionResult
    {
        public string Caption { get; set; }
        public byte[] Pic { get; set; }
        public NameGo[] NameGoes { get; set; }
        public int[] Columns { get; set; }
        public int Column
        {
            get => Columns != null && Columns.Length > 0 ? Columns[0] : 1;
            set => Columns = new int[value];
        }
        public string AutoGo { get; set; }
    }
}