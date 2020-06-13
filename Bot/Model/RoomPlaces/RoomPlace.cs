using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Model.RoomPlaces
{
    public class RoomPlace
    {
        public virtual IEnumerable<string> GoList => new string[0];
        public virtual async Task Visit(IBotMapVisitor visitor) { }
    }
}