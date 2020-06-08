using Bot.Model.Artifacts;
using Bot.Model.Rooms;

namespace Bot.Model
{
    public interface IBotMaestro
    {
        void Start();
        void GoToRoom(string roomKey);
        void Type(string message);
    }
}