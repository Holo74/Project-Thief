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
            // if (Input.IsActionJustPressed("ui_cancel"))
            // {
            //     // GD.Print("Captured");
            //     if (Input.MouseMode == Input.MouseModeEnum.Captured)
            //     {
            //         Input.MouseMode = Input.MouseModeEnum.Visible;
            //         Management.Game.GameManager.PLAYING = false;
            //     }
            //     else
            //     {
            //         Input.MouseMode = Input.MouseModeEnum.Captured;
            //         Management.Game.GameManager.PLAYING = true;
            //     }
            // }
        }
    }
}

