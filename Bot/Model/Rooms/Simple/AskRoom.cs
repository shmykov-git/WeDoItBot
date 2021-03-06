﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Model.RoomPlaces;
using Newtonsoft.Json;

namespace Bot.Model.Rooms.Simple
{
    public class AskRoom : PicRoom
    {
        public string Ask { get; set; }
        public string YesGo { get; set; }
        public string NoGo { get; set; }

        protected override void Simplify()
        {
            Places = new RoomPlace[]
            {
                new ButtonDialog()
                {
                    Caption = Ask,
                    Buttons = new []
                    {
                        new Button(){Go = YesGo, Name = "Да"},
                        new Button(){Go = NoGo, Name = "Нет"},
                    },
                    Columns = new[] {2}
                },
            };
        }

        [JsonIgnore]
        public override IEnumerable<string> GoList => new[] { YesGo, NoGo }.Concat(base.GoList);
    }
}