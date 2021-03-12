using System.Collections.Generic;
using System.Linq;
using Bot.Model.RoomPlaces;
using Newtonsoft.Json;
using Suit.Extensions;

namespace Bot.Model.Rooms.Simple
{
    public class MenuRoom : PicRoom
    {
        public string Menu { get; set; }
        public ListItem[] Items { get; set; }
        public string Col { get; set; }
        protected override void Simplify()
        {
            Places = new RoomPlace[]
            {
                new ReplyButtonDialog()
                {
                    Caption = Menu,
                    Buttons = Items.Select(item => new ReplyButton() {Name = item.Button, Go = item.Go}).ToArray(),
                    Columns = Col.SplitToInts(',', 1)
                },
            };
        }

        [JsonIgnore]
        public override IEnumerable<string> GoList => (Items?.Select(b => b.Go) ?? new string[0]).Concat(base.GoList);
    }
}