using Godot;
using System;

namespace Debug.Menus
{
    public partial class PlayerVariableDisplay : RichTextLabel
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            // if (Player.Variables.Instance.WALKING_MOVEMENT.Y > .1f || Player.Variables.Instance.WALKING_MOVEMENT.Y < -.1f)
            Text = "Speed: " + Player.Variables.Instance.WALKING_MOVEMENT + "\nGravity: " + Player.Variables.Instance.GRAVITY_MOVEMENT;
        }
    }

}
