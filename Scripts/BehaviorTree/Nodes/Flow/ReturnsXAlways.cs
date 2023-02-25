using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public class ReturnsXAlways : Base
    {
        [Export]
        private Results ReturnThis { get; set; }

        public override Results Tick(float delta, BehaviorController BC)
        {
            foreach (Base c in Children)
            {
                Results r = c.Tick(delta, BC);
                return ReturnThis;
            }
            return Results.Error;
        }
    }
}

