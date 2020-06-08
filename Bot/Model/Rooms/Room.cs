﻿using Bot.Model.Artifacts;

namespace Bot.Model.Rooms
{
    public class Room
    {
        public string Key { get; set; }
        public string AutoGo { get; set; }

        public virtual Artifact[] Artifacts => new Artifact[0];
        public virtual void Visit(IBotMapVisitor visitor) => visitor.VisitRoom(this);
    }
}