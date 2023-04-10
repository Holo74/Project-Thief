using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class SetPositionFromBB : Base
    {
        [Export]
        private Enums.KeyList Memory { get; set; }

        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            BC.BlackBoard[Enums.KeyList.Debugging] = "Setting position";
            if (BC.BlackBoard.ContainsKey(Memory) && BC.BlackBoard[Memory].VariantType == Variant.Type.Vector3)
            {
                BC.NavAgent.TargetPosition = BC.BlackBoard[Memory].AsVector3();
                return Results.Success;
            }
            return Results.Failure;
        }
    }

}
