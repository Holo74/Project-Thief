using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public class SetTimer : Base
    {
        [Export]
        private float TimerSet { get; set; }
        private Timer Clock { get; set; }

        public override void _Ready()
        {
            base._Ready();
            Clock = GetNode<Timer>("Timer");
        }

        public override Results Tick(float delta, BehaviorController BC)
        {
            Clock.Start(TimerSet);
            return Results.Success;
        }
    }

}
