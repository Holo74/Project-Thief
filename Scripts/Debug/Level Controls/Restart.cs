using Godot;
using System;

namespace Debug.LevelControls
{
    public partial class Restart : Button
    {
        [Export]
        private string ScenePath { get; set; }
        private Vector3 PlayerPosition { get; set; }

        public override void _Ready()
        {
            //PlayerPosition = Player.PlayerQuickAccess.CHARACTER_BODY.Transform3D.origin;
        }

        private void RestartLevel()
        {
            // Player.PlayerQuickAccess.CHARACTER_BODY.Position = PlayerPosition;
            // Player.Variables.Instance.GRAVITY_MOVEMENT = Vector3.Zero;
            Management.Game.GameManager.Instance.QuitToMainMenu();
        }
    }
}