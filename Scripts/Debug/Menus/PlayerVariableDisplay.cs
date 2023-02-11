using Godot;
using System;

namespace Debug.Menus
{
    public class PlayerVariableDisplay : RichTextLabel
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            // if (Player.Variables.WALKING_MOVEMENT.y > .1f || Player.Variables.WALKING_MOVEMENT.y < -.1f)
            Text = "Speed: " + Player.Variables.WALKING_MOVEMENT + "\nGravity: " + Player.Variables.GRAVITY_MOVEMENT;
        }
    }

}
