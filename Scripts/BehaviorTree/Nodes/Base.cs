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
                Controller = BC;
                GD.Print("Setting children up for failure");
                foreach (Base x in Children)
                {
                    x.SetController();

                }
            }
            if (parent is Base p && !DisabledBehaviour)
            {
                Parent = p;
                Parent.Children.Add(this);
            }
        }

        protected void SetController()
        {
            GD.Print("Parent is null: " + (Parent is null).ToString());
            Controller = Parent.Controller;
            foreach (Base child in Children)
            {
                child.SetController();
            }
            AfterControllerSet();
        }

        protected virtual void AfterControllerSet()
        {

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
