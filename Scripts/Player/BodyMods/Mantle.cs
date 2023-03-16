using Godot;
using System;

namespace Player.BodyMods
{
    public partial class Mantle : Node3D
    {
        public MantleRaycasters Upper { get; private set; }
        private MantleRaycasters Lower { get; set; }
        [Export]
        private Node3D LowerPosition { get; set; }
        private float PlayerMinSize { get; set; }

        [Export]
        public RayCast3D LeftHand { get; set; }
        [Export]
        public RayCast3D RightHand { get; set; }

        [Export]
        private float[] MinSizes { get; set; }
        [Export]
        private float[] CastToSizes { get; set; }
        private float AbsoluteMin { get; set; }
        public Vector3 MantleHeight { get; private set; }

        public override void _Ready()
        {
            Upper = GetNode<MantleRaycasters>("Positioner/Upper Raycasts");
            Lower = GetNode<MantleRaycasters>("Lower/Lower Raycasts");

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
                Vector3 holder = LowerPosition.GlobalPosition;
                holder.Y = Upper.FurthestHit().Y;
                LowerPosition.GlobalPosition = holder;
                if (Lower.GetSmallestDistance() > AbsoluteMin && (HandsCanHold() || !HandsAboveWaist()))
                {
                    holder.Y += 1.0f;
                    MantleHeight = holder;
                    return true;
                }
            }
            return false;
        }

        public bool HandsAboveWaist()
        {
            return Lower.GlobalPosition.Y > PlayerQuickAccess.KINEMATIC_BODY.GlobalPosition.Y;
        }

        private bool HandsCanHold()
        {
            return RightHand.IsColliding() && LeftHand.IsColliding() && RightHand.GetCollisionNormal().Dot(LeftHand.GetCollisionNormal()) > .9f;
        }
    }

}
