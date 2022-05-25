using Godot;
using System;

namespace Debug
{
    public class CrossProductTester : Node
    {
        private Vector3 Right { get; set; }
        private bool ForwardSwing = true;
        private float PreviousDot { get; set; }

        public override void _Ready()
        {
            base._Ready();
            KinematicBody moving = GetNode<KinematicBody>("RigidBody");
            Right = -moving.GlobalTransform.basis.z;
        }

        public override void _Process(float delta)
        {

        }

        public override void _PhysicsProcess(float delta)
        {
            KinematicBody moving = GetNode<KinematicBody>("RigidBody");
            Spatial stationary = GetNode<Spatial>("RigidBody2");
            Vector3 directionTo = stationary.GlobalTransform.origin - moving.GlobalTransform.origin;
            directionTo = directionTo.Normalized();

            //GD.Print(directionTo.Cross(-moving.GlobalTransform.basis.z));

            float dot = Vector3.Up.Dot((stationary.GlobalTransform.origin - moving.GlobalTransform.origin).Normalized());
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

            moving.MoveAndCollide(movingTo * delta * dot);
            PreviousDot = dot;
        }
    }
}

