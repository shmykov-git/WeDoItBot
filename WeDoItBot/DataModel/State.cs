using System.Collections.Concurrent;

namespace WeDoItBot.DataModel
{
    class State
    {
        public Room CurrentRoom { get; set; }
        public ConcurrentDictionary<string, string> Values = new ConcurrentDictionary<string, string>();
        public StateType StateType { get; set; } = StateType.Any;
        public Command CurrentCommand { get; set; }
    }
}