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

        // If the dot product between the safe velocity and the current facing direction are beyond this then turn
        [Export(PropertyHint.Range, ("0,360,1"))]
        private float StartBeyondXDegrees { get; set; }
        public override void _Ready()
        {
            BlackBoard = new Godot.Collections.Dictionary<BehaviorTree.Enums.KeyList, object>();
            NavAgent = GetNode<NavigationAgent>("NavigationAgent");
            AnimTree = GetNode<AnimationTree>("AnimationTree");
            StartBeyondXDegrees = (180 - StartBeyondXDegrees) / 180;
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

        public void AssignRoot(Nodes.Base root)
        {
            Root = root;
        }

        public void InArea(Node body, bool entered, BehaviorTree.Enums.KeyList name)
        {
            BlackBoard[name] = entered;
        }

        public void SafeVelocityComputed(Vector3 velocity)
        {
            if (velocity.Dot(-GlobalTransform.basis.z) < StartBeyondXDegrees)
            {

            }
        }
    }

}
