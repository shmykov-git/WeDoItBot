using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;

namespace Bot.Model
{
    public interface IBotMapVisitor
    {
        void VisitPicRoom(PicRoom picRoom);
        void VisitActionRoom(ActionRoom actionRoom);
        void VisitRoom(Room room);
        void VisitActionRoomPlace(ActionRoomPlace actionRoomPlace);
        void VisitButton(Button button);
        void VisitButtonDialog(ButtonDialog buttonDialog);
    }
}