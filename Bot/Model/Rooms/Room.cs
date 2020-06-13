using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public virtual IEnumerable<string> GoList => new[] {AutoGo}.Where(g => g != null);

        public virtual async Task Visit(IBotMapVisitor visitor)
        {
            await visitor.VisitRoom(this);
        }
    }
}