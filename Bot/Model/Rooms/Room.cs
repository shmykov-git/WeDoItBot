using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Model.Artifacts;
using Suit.Extensions;

namespace Bot.Model.Rooms
{
    public class Room
    {
        private string key;

        public string Id { get; set; }

        public string Key
        {
            get => key ?? Id;
            set => key = value;
        }

        public string AutoGo { get; set; }

        public virtual IEnumerable<string> GoList => new[] {AutoGo}.Where(g => g != null);

        public virtual async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitRoom(this);
        }
    }
}