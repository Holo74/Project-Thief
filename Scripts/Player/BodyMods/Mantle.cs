using Godot;
using System;

namespace Player.BodyMods
{
    public partial class Mantle : Node3D
    {
        private MantleRaycasters Upper { get; set; }
        private MantleRaycasters Lower { get; set; }
        private float PlayerMinSize { get; set; }
        [Export]
        private float[] MinSizes { get; set; }
        [Export]
        private float[] CastToSizes { get; set; }
        private float AbsoluteMin { get; set; }
        public Vector3 MantleHeight { get; private set; }

        public override void _Ready()
        {
            Upper = GetNode<MantleRaycasters>("Positioner/Upper Raycasts");
            Lower = GetNode<MantleRaycasters>("Lower Raycasts");
            Variables.Instance.StandingChangedTo += SetMinSize;
            PlayerMinSize = MinSizes[0];
            AbsoluteMin = 1f;
        }

        private void SetMinSize(Variables.PlayerStandingState state)
        {
            int index = ((int)state);
            PlayerMinSize = MinSizes[index];
            Upper.SetTargetPosition(Vector3.Down * CastToSizes[index]);
            Lower.SetTargetPosition(Vector3.Up * CastToSizes[index]);
        }

        public bool CanMantle()
        {
            if (Upper.IsColliding())
            {
                Vector3 holder = Lower.GlobalPosition;
                holder.Y = Upper.FurthestHit().Y;
                Lower.GlobalPosition = holder;
                GD.Print(Lower.GetSmallestDistance());
                if (Lower.GetSmallestDistance() > AbsoluteMin)
                {
                    holder.Y += 0.1f;
                    MantleHeight = holder;
                    return true;
                }
            }
            return false;
        }
    }

}
