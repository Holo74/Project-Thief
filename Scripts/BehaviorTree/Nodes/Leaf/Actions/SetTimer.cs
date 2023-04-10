using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class SetTimer : Base
    {
        [Export]
        private float TimerSet { get; set; }
        private Timer Clock { get; set; }

        public override void _Ready()
        {
            base._Ready();
            Clock = GetNode<Timer>("Timer");
        }

        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            BC.BlackBoard[Enums.KeyList.Debugging] = "Setting timer";
            Clock.Start(TimerSet);
            return Results.Success;
        }
    }

}
