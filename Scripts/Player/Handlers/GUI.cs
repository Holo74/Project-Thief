using Godot;
using System;

namespace Player.Handlers
{
    public class GUI : Node
    {

        public override void _Ready()
        {

        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                // GD.Print("Captured");
                if (Input.GetMouseMode() == Input.MouseMode.Captured)
                {
                    Input.SetMouseMode(Input.MouseMode.Visible);
                    Management.Game.GameManager.PLAYING = false;
                }
                else
                {
                    Input.SetMouseMode(Input.MouseMode.Captured);
                    Management.Game.GameManager.PLAYING = true;
                }
            }
        }
    }
}

