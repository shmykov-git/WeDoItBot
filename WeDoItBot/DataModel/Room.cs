using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace WeDoItBot.DataModel
{
    enum CommandType
    {
        Ask,
        Go,
        Link,
        Enter
    }

    class Command
    {
        public CommandType Type { get; set; }
        public string Name { get; set; }
        public string Question { get; set; }
        public string NotImplMsg { get; set; }
        public string NameYes { get; set; }
        public string NameNo { get; set; }
        public string GoYes { get; set; }
        public string GoNo { get; set; }
        public string Go { get; set; }
        public string Link { get; set; }
        public string ValueKey { get; set; }
    }

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

    class BotMap
    {
        public Room[] Rooms { get; set; }
    }

    class State
    {
        public Room CurrentRoom { get; set; }
        public ConcurrentDictionary<string, string> Values = new ConcurrentDictionary<string, string>();
        public StateType StateType { get; set; } = StateType.Any;
        public Command CurrentCommand { get; set; }
    }

    enum StateType
    {
        Any,
        WaitingForAnswer
    }
}
