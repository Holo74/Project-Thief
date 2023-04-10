using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class MoveToPosition : Base
    {
        [Export]
        private float Speed { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            BC.SetVelocityToPhysics(BC.GlobalPosition.DirectionTo(BC.NavAgent.GetNextPathPosition()) * Speed);
            // GD.Print(BC.NavAgent.GetNextPathPosition());
            BC.BlackBoard[Enums.KeyList.Debugging] = "Moving";
            return Results.Success;
        }
    }

}
