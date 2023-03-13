using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public partial class Flipper : Base
    {
        public override Results Tick(double delta, BehaviorController BC)
        {
            foreach (Base c in Children)
            {
                Results r = c.Tick(delta, BC);
                switch (r)
                {
                    case Results.Failure:
                        return Results.Success;
                    case Results.Success:
                        return Results.Failure;
                    default:
                        return Results.Runnings;
                }
            }
            return Results.Error;
        }
    }

}
