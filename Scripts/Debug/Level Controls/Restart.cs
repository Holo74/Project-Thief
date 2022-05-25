using Godot;
using System;

namespace Debug.LevelControls
{
    public class Restart : Button
    {
        [Export]
        private string ScenePath { get; set; }
        private Vector3 PlayerPosition { get; set; }

        public override void _Ready()
        {
            //PlayerPosition = Player.PlayerQuickAccess.KINEMATIC_BODY.Transform.origin;
        }

        private void RestartLevel()
        {
            // Player.PlayerQuickAccess.KINEMATIC_BODY.Translation = PlayerPosition;
            // Player.Variables.GRAVITY_MOVEMENT = Vector3.Zero;
            GetTree().ChangeScene("res://Scenes/Menus/MainMenu.tscn");
        }
    }
}