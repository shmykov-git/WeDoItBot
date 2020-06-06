namespace WeDoItBot.DataModel
{
    class Command
    {
        public CommandType Type { get; set; }
        public string Name { get; set; }
        public string Question { get; set; }
        public string NotImplMsg { get; set; }
        public string NameYes { get; set; }
        public string NameNo { get; set; }
        public string GoYes { get; set; }
        public string GoNo { get; set; }
        public string Go { get; set; }
        public string Link { get; set; }
        public string ValueKey { get; set; }
    }
}