using System.Threading.Tasks;
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

        public Task VisitActionRoom(ActionRoom actionRoom)
        {
            log.Debug($"");
            log.Debug($"{actionRoom.ActionName}(\"{actionRoom.ActionArgument}\");");

            return Task.CompletedTask;
        }

        public Task VisitRoom(Room room)
        {
            if (room.AutoGo.IsNotNullOrEmpty())
                log.Debug($"AutoGo: {room.AutoGo}");

            return Task.CompletedTask;
        }

        public Task VisitActionRoomPlace(ActionRoomPlace actionRoomPlace)
        {
            log.Debug($"{actionRoomPlace.ActionName}(\"{actionRoomPlace.ActionArgument}\");");

            return Task.CompletedTask;
        }

        public Task VisitButton(Button button)
        {
            log.Debug(button.Caption);
            log.Debug($"<Button key=\"{button.Key}\">{button.Name}</Button>");

            return Task.CompletedTask;
        }

        public Task VisitButtonDialog(ButtonDialog buttonDialog)
        {
            log.Debug($"???{buttonDialog.Question}");
            buttonDialog.Buttons.ForEach(async b => await VisitButton(b));

            return Task.CompletedTask;
        }

        public Task VisitPicRoom(PicRoom picRoom)
        {
            log.Debug("");
            log.Debug($"({picRoom.Key})");
            log.Debug(picRoom.Name);
            log.Debug($"^^{picRoom.Pic}^^");
            log.Debug(picRoom.Description);

            return Task.CompletedTask;
        }
    }
}