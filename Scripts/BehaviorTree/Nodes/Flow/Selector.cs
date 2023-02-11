using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public class Selector : Base
    {
        public override Results Tick(float delta, BehaviorController BC)
        {
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
