using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Conditions
{
    public partial class TimerReader : Base
    {
        // This will read once and then flip back to being false
        [Export]
        private double Timer { get; set; }
        private double CurrentTime { get; set; }
        private int ResetTime { get; set; }
        [Export]
        private bool WaitInitial { get; set; }

        public override void _Ready()
        {
            base._Ready();
            CurrentTime = WaitInitial ? Timer : 0;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (ResetTime > 0)
            {
                ResetTime--;
                if (ResetTime == 0)
                {
                    CurrentTime = Timer;
                }
            }
        }

        public override Results Tick(double delta, BehaviorController BC)
        {
            ResetTime = 60;
            base.Tick(delta, BC);

            CurrentTime -= delta;
            if (CurrentTime < 0)
            {
                CurrentTime = Timer;
                return Results.Success;
            }
            return Results.Failure;
        }
    }
}

