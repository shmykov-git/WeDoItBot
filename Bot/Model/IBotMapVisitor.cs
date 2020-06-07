using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;

namespace Bot.Model
{
    public interface IBotMapVisitor
    {
        void ShowPicRoom(PicRoom picRoom);
        void DoAction(ActionRoom actionRoom);
        void ShowButton(Button button);
        void ShowButtonDialog(ButtonDialog buttonDialog);
    }
}