using Godot;
using System;

namespace BehaviorTree
{
    public class BehaviorController : KinematicBody
    {
        private Nodes.Base Root { get; set; }
        public Godot.Collections.Dictionary<BehaviorTree.Enums.KeyList, object> BlackBoard { get; private set; }
        public NavigationAgent NavAgent { get; private set; }
        public AnimationTree AnimTree { get; private set; }
        public Vector3 SetVelocity { get; set; }

        private int VelocitySyncCounter { get; set; }

        [Export]
        private float TurnSpeed { get; set; }

        // If the dot product between the safe velocity and the current facing direction are beyond this then turn
        [Export(PropertyHint.Range, ("0,180,1"))]
        private float StartBeyondXDegrees { get; set; }
        public override void _Ready()
        {
            BlackBoard = new Godot.Collections.Dictionary<BehaviorTree.Enums.KeyList, object>();
            NavAgent = GetNode<NavigationAgent>("NavigationAgent");
            AnimTree = GetNode<AnimationTree>("AnimationTree");
            StartBeyondXDegrees = (90 - StartBeyondXDegrees) / 90;
            VelocitySyncCounter = 0;
        }

        public override void _Process(float delta)
        {
            if (!(Root is null))
            {
                Nodes.Results result = Root.Tick(delta, this);
                if (result == Nodes.Results.Failure)
                {
                    GD.Print("Ended in Failure");
                }
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (SetVelocity.IsEqualApprox(Vector3.Zero))
            {
                NavAgent.SetVelocity(SetVelocity);
                VelocitySyncCounter++;
                if (VelocitySyncCounter >= 60)
                {
                    SetVelocity = Vector3.Zero;
                }
            }
        }

        public void AssignRoot(Nodes.Base root)
        {
            Root = root;
        }

        public void InArea(Node body, bool entered, BehaviorTree.Enums.KeyList name)
        {
            BlackBoard[name] = entered;
        }

        public void SetVelocityToPhysics(Vector3 vel)
        {
            SetVelocity = vel;
            VelocitySyncCounter = 0;
        }

        public void SafeVelocityComputed(Vector3 velocity)
        {
            // Turn to
            if (velocity.Dot(-GlobalTransform.basis.z) < StartBeyondXDegrees)
            {
                Transform = Transform.InterpolateWith(Transform.LookingAt(velocity, Vector3.Up), GetPhysicsProcessDeltaTime() * TurnSpeed);
            }
            else
            {
                MoveAndSlideWithSnap(velocity, Vector3.Down * .4f, Vector3.Up, true);
            }
        }
    }

}
