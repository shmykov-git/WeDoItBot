using Bot.Model;
using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;
using Suit.Extensions;
using Suit.Logs;

namespace Bot.Test
{
    public class TestBotMapVisitor: IBotMapVisitor
    {
        private readonly ILog log;

        public TestBotMapVisitor(ILog log)
        {
            this.log = log;
        }

        public void VisitActionRoom(ActionRoom actionRoom)
        {
            log.Debug($"");
            log.Debug($"{actionRoom.ActionName}(\"{actionRoom.ActionArgument}\");");
        }

        public void VisitRoom(Room room)
        {
            if (room.AutoGo.IsNotNullOrEmpty())
                log.Debug($"AutoGo: {room.AutoGo}");
        }

        public void VisitActionRoomPlace(ActionRoomPlace actionRoomPlace)
        {
            log.Debug($"{actionRoomPlace.ActionName}(\"{actionRoomPlace.ActionArgument}\");");
        }

        public void VisitButton(Button button)
        {
            log.Debug($"<Button key=\"{button.Key}\">{button.Name}</Button>");
        }

        public void VisitButtonDialog(ButtonDialog buttonDialog)
        {
            log.Debug($"???{buttonDialog.Question}");
            buttonDialog.Buttons.ForEach(VisitButton);
        }

        public void VisitPicRoom(PicRoom picRoom)
        {
            log.Debug("");
            log.Debug($"({picRoom.Key})");
            log.Debug(picRoom.Name);
            log.Debug($"^^{picRoom.Pic}^^");
            log.Debug(picRoom.Description);
        }
    }
}