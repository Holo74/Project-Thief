using Godot;
using System;

namespace BehaviorTree.Nodes.Flow
{
    public partial class ReturnsXAlways : Base
    {
        [Export]
        private Results ReturnThis { get; set; }

        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            foreach (Base c in Children)
            {
                Results r = c.Tick(delta, BC);
            }
            return ReturnThis;
        }
    }
}

