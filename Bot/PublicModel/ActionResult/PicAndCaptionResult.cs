namespace Bot.PublicModel.ActionResult
{
    public class PicAndCaptionResult : ActionResult
    {
        public string Caption { get; set; }
        public byte[] Pic { get; set; }
    }
}