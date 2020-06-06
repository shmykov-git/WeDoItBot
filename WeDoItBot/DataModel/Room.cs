using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace WeDoItBot.DataModel
{
    class Room
    {
        public string Message { get; set; }
        public string Message2 { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Pic { get; set; }
        public Command[] Commands { get; set; }
    }
}
