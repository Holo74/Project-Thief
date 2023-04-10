using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public partial class Sequencer : Base
    {
        public override void _Ready()
        {
            base._Ready();
            OGName = "Sequencer";
        }
        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            foreach (Base c in Children)
            {
                Results r = c.Tick(delta, BC);
                if (r == Results.Failure || r == Results.Runnings)
                {
                    return r;
                }
            }
            return Results.Success;
        }
    }
}
