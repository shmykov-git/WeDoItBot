using Bot.Model.Artifacts;
using Bot.Model.Rooms;

namespace Bot.Model
{
    public interface IBotMaestro
    {
        void Start();
        Artifact[] GoToRoom(string roomKey);
        void Say(string message);
    }
}