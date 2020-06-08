using Bot.Model.Artifacts;
using Bot.Model.Rooms;

namespace Bot.Model
{
    public interface IBotMaestro
    {
        void Start();
        void Command(string command);
        void Type(string message);
    }
}