using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bot.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StateType
    {
        None,
        WaitingForAnswer
    }
}