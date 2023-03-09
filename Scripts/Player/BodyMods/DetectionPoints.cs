using Godot;
using System;

namespace Player.BodyMods
{
    public partial class DetectionPoints : Node
    {
        private CollisionShape3D Head { get; set; }
        private CollisionShape3D Body { get; set; }
        private CollisionShape3D Feet { get; set; }
        public override void _Ready()
        {
            Head = GetNode<CollisionShape3D>("Head");
            Body = GetNode<CollisionShape3D>("Body");
            Feet = GetNode<CollisionShape3D>("Legs");
            Variables.Instance.StandingChangedTo += UpdateCollisionDelayer;

        }

        private void UpdateCollisionDelayer(Variables.PlayerStandingState state)
        {
            CallDeferred(nameof(UpdateCollision), Variant.From<Variables.PlayerStandingState>(state));
        }

        private void UpdateCollision(Variables.PlayerStandingState state)
        {
            Head.Disabled = PlayerQuickAccess.UPPER_BODY.Disabled;
            Body.Disabled = PlayerQuickAccess.LOWER_BODY.Disabled;
            Feet.Disabled = PlayerQuickAccess.FEET.Disabled;
        }
    }

}
