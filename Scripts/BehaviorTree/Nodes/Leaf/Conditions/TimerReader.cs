using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public class TimerReader : Base
    {
        // This will read once and then flip back to being false
        [Export]
        private bool TimerWentOff { get; set; }
        public override Results Tick(float delta, BehaviorController BC)
        {
            if (TimerWentOff)
            {
                TimerWentOff = false;
                return Results.Success;
            }
            return Results.Failure;
        }

        public void TimedOut()
        {
            TimerWentOff = true;
        }
    }
}

