using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class SetTargetToHome : SetTarget
    {
        [Export]
        private Vector3 GlobalPositioning;
        [Export]
        private Marker3D Position { get; set; }
        public override void _Ready()
        {
            base._Ready();
            GlobalPositioning = Position.GlobalPosition;
        }
        protected override bool SetTargetPosition()
        {
            Target = "Set position";
            TargetPosition = GlobalPositioning;
            return true;
        }
    }

}
