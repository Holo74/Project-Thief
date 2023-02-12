using Godot;
using System;

namespace BehaviorTree
{
    public class BehaviorController : KinematicBody
    {
        private Nodes.Base Root { get; set; }
        public Godot.Collections.Dictionary<KeyList, object> BlackBoard { get; private set; }
        public NavigationAgent NavAgent { get; private set; }
        public AnimationTree AnimTree { get; private set; }
        public override void _Ready()
        {
            BlackBoard = new Godot.Collections.Dictionary<KeyList, object>();
            NavAgent = GetNode<NavigationAgent>("NavigationAgent");
            AnimTree = GetNode<AnimationTree>("AnimationTree");
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

        public void InArea(Node body, bool entered, KeyList name)
        {
            BlackBoard[name] = entered;
        }
    }

}
