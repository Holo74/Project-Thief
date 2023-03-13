using Godot;
using System;

namespace Environment.Areas
{
    public partial class SwingingVine : PlayerArea
    {
        private Vector3 Point { get; set; }

        public override void _Ready()
        {
            base._Ready();
            Point = GetNode<Node3D>("Swing Point").GlobalPosition;
        }

        protected override void PlayerEntered()
        {
            if (!(Player.Variables.Instance.MOVEMENT is Player.Movement.Swinging))
            {
                Player.Variables.Instance.MOVEMENT = new Player.Movement.Swinging(Point, GetNode<Node3D>("Swing Point").GlobalTransform.Basis.Z);
            }
        }

        protected override void PlayerLeft()
        {

        }

    }

}
