using Godot;
using System;

namespace Environment.Areas
{
    public class SwingingVine : PlayerArea
    {
        private Vector3 Point { get; set; }

        public override void _Ready()
        {
            base._Ready();
            Point = GetNode<Spatial>("Swing Point").GlobalTransform.origin;
        }

        protected override void PlayerEntered()
        {
            if (!(Player.Variables.MOVEMENT is Player.Movement.Swinging))
            {
                Player.Variables.MOVEMENT = new Player.Movement.Swinging(Point, GetNode<Spatial>("Swing Point").GlobalTransform.basis.z);
            }
        }

        protected override void PlayerLeft()
        {

        }

    }

}
