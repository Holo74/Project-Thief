using Godot;
using System;

namespace Debug
{
    public partial class CrossProductTester : Node
    {
        private Vector3 Right { get; set; }
        private bool ForwardSwing = true;
        private float PreviousDot { get; set; }

        public override void _Ready()
        {
            base._Ready();
            CharacterBody3D moving = GetNode<CharacterBody3D>("RigidBody3D");
            Right = -moving.GlobalTransform.Basis.Z;
        }

        public override void _Process(double delta)
        {

        }

        public override void _PhysicsProcess(double delta)
        {
            CharacterBody3D moving = GetNode<CharacterBody3D>("RigidBody3D");
            Node3D stationary = GetNode<Node3D>("RigidBody2");
            Vector3 directionTo = stationary.GlobalPosition - moving.GlobalPosition;
            directionTo = directionTo.Normalized();

            //GD.Print(directionTo.Cross(-moving.GlobalTransform.Basis.Z));

            float dot = Vector3.Up.Dot((stationary.GlobalPosition - moving.GlobalPosition).Normalized());
            if (dot < .8f)
            {
                if (dot < PreviousDot)
                {
                    ForwardSwing = !ForwardSwing;
                    Right = -Right;
                    GD.Print("Dot current: " + dot + "\nPrevious Dot: " + PreviousDot);
                }
            }
            Vector3 movingTo = directionTo.Cross(Right);
            GD.Print(movingTo);

            moving.MoveAndCollide(movingTo * ((float)delta) * dot);
            PreviousDot = dot;
        }
    }
}

