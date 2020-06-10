using System.Collections.Concurrent;
using Bot.Model.Rooms;

namespace Bot.Model
{
    public class State
    {
        public Room CurrentRoom { get; set; }
        public ConcurrentDictionary<string, string> Values = new ConcurrentDictionary<string, string>();
        public StateType StateType { get; set; } = StateType.None;
    }
}