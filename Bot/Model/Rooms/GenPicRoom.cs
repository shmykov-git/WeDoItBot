using System.Linq;
using System.Threading.Tasks;
using Bot.Model.RoomPlaces;
using Bot.PublicModel.ActionResult;

namespace Bot.Model.Rooms
{
    public class GenPicRoom : ShowRoom
    {
        public string ActionName { get; set; }
        public string ActionArgument { get; set; }

        public string Caption { get; set; }
        public string Go { get; set; }

        protected virtual void Simplify() { }

        public void Generate(PicAndCaptionResult picGen)
        {
            if (picGen.NameGoes.Any())
            {
                Places = new RoomPlace[]
                {
                    new ButtonDialog()
                    {
                        Caption = picGen.Caption ?? Caption,
                        Buttons = picGen.NameGoes.Select(ng=>new Button(){Name = ng.Name, Go = ng.Go}).ToArray(),
                        ColumnsCount = picGen.ColumnsCount
                    },
                };
            }
            else
            {
                Places = new RoomPlace[]
                {
                    new Button()
                    {
                        Caption = picGen.Caption ?? Caption,
                        Name = "Хорошо",
                        Go = Go
                    },
                };
            }
        }

        public override async Task Visit(IBotMapVisitor visitor)
        {
            Simplify();

            await visitor.VisitGenRoom(this);
            await base.Visit(visitor);
        }
    }
}