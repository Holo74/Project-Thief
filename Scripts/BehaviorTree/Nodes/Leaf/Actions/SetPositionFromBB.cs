using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public class SetPositionFromBB : Base
    {
        [Export]
        private Enums.KeyList Memory { get; set; }

        public override Results Tick(float delta, BehaviorController BC)
        {
            if (BC.BlackBoard.ContainsKey(Memory) && BC.BlackBoard[Memory] is Vector3 vec)
            {
                BC.BlackBoard[Enums.KeyList.MoveToPosition] = vec;
                return Results.Success;
            }
            return Results.Failure;
        }
    }

}
