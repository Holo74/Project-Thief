using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public partial class Selector : Base
    {
        public override void _Ready()
        {
            base._Ready();
            OGName = "Selector";
        }
        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            foreach (Base c in Children)
            {
                Results r = c.Tick(delta, BC);
                if (r == Results.Success || r == Results.Runnings)
                {
                    return r;
                }
            }
            return Results.Failure;
        }
    }

}
