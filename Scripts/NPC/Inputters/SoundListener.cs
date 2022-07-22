using Godot;
using System;

namespace NPC.Inputters
{
    public class SoundListener : Spatial
    {
        public override void _Ready()
        {
            AddToGroup("Listener");
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

        }
    }

}
