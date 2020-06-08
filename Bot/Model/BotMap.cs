using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Model.Rooms;
using Suit.Extensions;

namespace Bot.Model
{
    public class BotMap
    {
        public Room[] Rooms { get; set; }

        public virtual async Task Visit(IBotMapVisitor visitor)
        {
            Rooms?.ForEach(async r=> await r.Visit(visitor));
        }
    }
}
