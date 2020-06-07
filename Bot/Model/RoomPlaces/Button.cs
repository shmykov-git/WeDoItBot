﻿namespace Bot.Model.RoomPlaces
{
    public class Button : RoomPlace
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public override void Visit(IBotMapVisitor visitor)
        {
            visitor.ShowButton(this);

            base.Visit(visitor);
        }
    }
}