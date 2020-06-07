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

        private void GoToRoom(string key)
        {
            log.Debug($"=> {key}");
        }

        public void DoAction(ActionRoom actionRoom)
        {
            log.Debug($"{actionRoom.ActionName}({actionRoom.ActionArgument});");
            GoToRoom(actionRoom.Go);
        }

        public void ShowButton(Button button)
        {
            log.Debug($"<Button key={button.Key}>{button.Name}</Button>");
        }

        public void ShowButtonDialog(ButtonDialog buttonDialog)
        {
            log.Debug($"???{buttonDialog.Question}");
            buttonDialog.Buttons.ForEach(ShowButton);
        }

        public void ShowPicRoom(PicRoom picRoom)
        {
            log.Debug(picRoom.Name);
            log.Debug($"^^{picRoom.Pic}^^");
            log.Debug(picRoom.Description);
        }
    }
}