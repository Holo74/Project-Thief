using Godot;
using System;

namespace Environment.Areas
{
    public class WebTrap : PlayerArea
    {
        [Export]
        private float WalkSpeed { get; set; }
        [Export]
        private float JumpStr { get; set; }

        public override void _Ready()
        {
            base._Ready();
        }

        protected override void PlayerEntered()
        {
            Player.Variables.SPEED_MOD = WalkSpeed;
            Player.Variables.JUMP_MOD = JumpStr;
        }

        protected override void PlayerLeft()
        {
            Player.Variables.RESET_SPEED_MOD();
            Player.Variables.RESET_JUMP_MOD();
        }
    }

}
