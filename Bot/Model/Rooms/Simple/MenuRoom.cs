﻿using System.Collections.Generic;
using System.Linq;
using Bot.Model.RoomPlaces;
using Newtonsoft.Json;

namespace Bot.Model.Rooms.Simple
{
    public class MenuRoom : PicRoom
    {
        public string Menu { get; set; }
        public ListItem[] Items { get; set; }
        public int? ColumnsCount { get; set; }
        public int? Col { get; set; }
        protected override void Simplify()
        {
            Places = new RoomPlace[]
            {
                new ReplyButtonDialog()
                {
                    Caption = Menu,
                    Buttons = Items.Select(item => new ReplyButton() {Name = item.Button, Go = item.Go}).ToArray(),
                    ColumnsCount = Col ?? ColumnsCount ?? 1
                },
            };
        }

        [JsonIgnore]
        public override IEnumerable<string> GoList => (Items?.Select(b => b.Go) ?? new string[0]).Concat(base.GoList);
    }
}