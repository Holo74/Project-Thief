using Godot;
using System.Collections.Generic;

namespace BehaviorTree.Nodes
{

    public abstract partial class Base : Node
    {
        protected string OGName { get; set; }
        private int CountDown = 10;
        private Base Parent { get; set; }
        protected List<Base> Children { get; set; } = new List<Base>();
        protected BehaviorController Controller { get; set; }
        [Export]
        private bool DisabledBehaviour { get; set; }

        public override void _Ready()
        {
            OGName = Name;
            Node parent = GetParent();
            if (parent is BehaviorController BC)
            {
                BC.AssignRoot(this);
            }
            if (parent is Base p && !DisabledBehaviour)
            {
                Parent = p;
                Parent.Children.Add(this);
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (CountDown > 0)
            {
                CountDown--;
                if (CountDown == 0)
                {
                    Name = OGName;
                }
            }
        }

        public virtual Results Tick(double delta, BehaviorController BC)
        {
            Name = OGName + " Running";
            CountDown = 10;
            return Results.Error;
        }
    }

    public enum Results
    {
        Failure,
        Success,
        Runnings,
        Error
    }

}
