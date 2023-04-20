using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class SetHomeToStart : Base
    {
        [Export]
        private Node3D Location { get; set; }
        private Vector3 Home { get; set; }

        public override void _Ready()
        {
            base._Ready();
            Home = Location.GlobalPosition;
        }
        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            BC.NavAgent.TargetPosition = Home;
            return Results.Success;
        }
    }

}