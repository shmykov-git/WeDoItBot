﻿using System.Threading.Tasks;
using Bot.Model.RoomPlaces;
using Bot.Model.Rooms;

namespace Bot.Model
{
    public interface IBotMapVisitor
    {
        Task VisitPicRoom(PicRoom picRoom);
        Task VisitActionRoom(ActionRoom actionRoom);
        Task VisitRoom(Room room);
        Task VisitActionRoomPlace(ActionRoomPlace actionRoomPlace);
        Task VisitButton(Button button);
        Task VisitReplyButton(ReplyButton button);
        Task VisitButtonDialog(ButtonDialog buttonDialog);
        Task VisitReplyButtonDialog(ReplyButtonDialog buttonDialog);
        Task VisitButtonMenu(ButtonMenu buttonDialog);
        Task EnterPlace(EnterPlace enterPlace);
        Task VisitGenRoom(GenPicRoom genPicRoom);
    }
}