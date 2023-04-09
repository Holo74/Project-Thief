using Godot;
using System;

namespace Player.Handlers
{
    public partial class Resistances : Resource
    {
        public Resistances()
        {

        }
        [Export]
        public Health.InteractionTypes resistant;
        [Export]
        public int amount;
    }
}
