using Godot;
using System.Collections.Generic;

namespace BehaviorTree.Nodes
{
    public abstract partial class Base : Node
    {
        private Base Parent { get; set; }
        protected List<Base> Children { get; set; }
        protected BehaviorController Controller { get; set; }

        public override void _Ready()
        {
            Children = new List<Base>();

            Node parent = GetParent();
            if (parent is BehaviorController BC)
            {
                BC.AssignRoot(this);
            }
            if (parent is Base p)
            {
                Parent = p;
                Parent.Children.Add(this);
            }
        }

        public abstract Results Tick(double delta, BehaviorController BC);
    }

    public enum Results
    {
        Failure,
        Success,
        Runnings,
        Error
    }

}
