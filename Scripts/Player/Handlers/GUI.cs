using Godot;
using System;

namespace Player.Handlers
{
    public partial class GUI : Node
    {

        public override void _Ready()
        {

        }

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {

            }
        }
    }
}

