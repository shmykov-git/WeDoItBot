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
            if (button.Caption.IsNotNullOrEmpty())
                log.Debug(button.Caption);

            log.Debug($"<Button key=\"{button.Go}\">{button.Name}</Button>");

            return Task.CompletedTask;
        }

        public Task VisitButtonDialog(ButtonDialog buttonDialog)
        {
            log.Debug($"{buttonDialog.Caption}, cols:{buttonDialog.ColumnsCount}");
            buttonDialog.Buttons.ForEach(async b => await VisitButton(b));

            return Task.CompletedTask;
        }

        public Task VisitButtonMenu(ButtonMenu buttonDialog)
        {
            log.Debug($"{buttonDialog.Caption}, cols:{buttonDialog.ColumnsCount}");
            log.Debug("========================");
            buttonDialog.Buttons.ForEach(async b => await VisitButton(b));

            return Task.CompletedTask;
        }

        public Task EnterPlace(EnterPlace enterPlace)
        {
            log.Debug($"{enterPlace.Name} ({enterPlace.Key}): _____");

            return Task.CompletedTask;
        }

        public Task VisitGenRoom(GenPicRoom genPicRoom)
        {
            log.Debug($"");
            log.Debug($"GEN: {genPicRoom.ActionName}(\"{genPicRoom.ActionArgument}\");");

            return Task.CompletedTask;
        }

        public Task VisitPicRoom(PicRoom picRoom)
        {
            log.Debug("");
            log.Debug($"({picRoom.Key})");

            if (picRoom.Name.IsNotNullOrEmpty())
                log.Debug(picRoom.Name);

            if (picRoom.Pic.IsNotNullOrEmpty())
                log.Debug($"^^{picRoom.Pic}^^");

            if (picRoom.Caption.IsNotNullOrEmpty())
                log.Debug(picRoom.Caption);

            return Task.CompletedTask;
        }
    }
}