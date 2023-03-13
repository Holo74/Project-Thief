using Godot;
using System;

namespace Environment.Areas
{
    public partial class WebTrap : PlayerArea
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
            Player.Variables.Instance.SPEED_MOD = WalkSpeed;
            Player.Variables.Instance.JUMP_MOD = JumpStr;
        }

        protected override void PlayerLeft()
        {
            Player.Variables.Instance.RESET_SPEED_MOD();
            Player.Variables.Instance.RESET_JUMP_MOD();
        }
    }

}
