namespace Bot.PublicModel.ActionResult
{
    public class PicAndCaptionResult : ActionResult
    {
        public string Caption { get; set; }
        public byte[] Pic { get; set; }
        public NameGo[] NameGoes { get; set; }
        public int ColumnsCount { get; set; } = 1;
        public string AutoGo { get; set; }
    }
}