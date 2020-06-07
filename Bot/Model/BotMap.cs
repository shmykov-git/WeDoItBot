using System.Collections.Generic;
using Bot.Model.Rooms;
using Suit.Extensions;

namespace Bot.Model
{
    public class BotMap
    {
        public Room[] Rooms { get; set; }

        public virtual void Visit(IBotMapVisitor visitor)
        {
            Rooms?.ForEach(r=>r.Visit(visitor));
        }
    }
}
