using System.Collections.Concurrent;
using Bot.Model.Rooms;
using Bot.Model.Rooms.Simple;
using Suit.Aspects;

namespace Bot.Model
{
    public class State
    {
        public Room CurrentRoom { get; set; }
        public MenuRoom LastMenuRoom { get; set; }
        public ConcurrentDictionary<string, string> Values = new ConcurrentDictionary<string, string>();
        public StateType StateType { get; [LoggingAspect(LoggingRule.Input)]set; } = StateType.None;
    }
}