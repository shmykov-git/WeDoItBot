using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Model.RoomPlaces;
using Newtonsoft.Json;

namespace Bot.Model.Rooms
{
    public class ShowRoom: Room
    {
        public RoomPlace[] Places { get; set; }

        [JsonIgnore]
        public EnterPlace EnterPlace { get; set; }

        public override IEnumerable<string> GoList =>
            (Places?.SelectMany(p => p.GoList) ?? new string[0]).Concat(base.GoList);

        public override async Task Visit(IBotMapVisitor visitor)
        {
            if (Places != null)
                foreach (var place in Places)
                {
                    if (EnterPlace == null)
                    {
                        await place.Visit(visitor);

                        if (place is EnterPlace enterPlace)
                        {
                            EnterPlace = enterPlace;

                            break;
                        }
                    }
                    else
                    {
                        if (place == EnterPlace)
                            EnterPlace = null;
                    }
                }

            await base.Visit(visitor);
        }
    }
}