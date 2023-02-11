using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public class Sequencer : Base
    {
        public override Results Tick(float delta, BehaviorController BC)
        {
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