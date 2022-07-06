using Godot;
using System;

namespace Player.Handlers
{
    public class Resistances : Resource
    {
        [Export]
        public Health.InteractionTypes resistant;
        [Export]
        public int amount;
    }
}
