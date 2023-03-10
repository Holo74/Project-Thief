using Godot;
using System;

namespace Player.BodyMods
{
    public class DetectionPoints : Node
    {
        private CollisionShape Head { get; set; }
        private CollisionShape Body { get; set; }
        private CollisionShape Feet { get; set; }
        public override void _Ready()
        {
            Head = GetNode<CollisionShape>("Head");
            Body = GetNode<CollisionShape>("Body");
            Feet = GetNode<CollisionShape>("Legs");
            Variables.Instance.StandingChangedTo += UpdateCollisionDelayer;

        }

        private void UpdateCollisionDelayer(Variables.PlayerStandingState state)
        {
            CallDeferred(nameof(UpdateCollision), state);
        }

        private void UpdateCollision(Variables.PlayerStandingState state)
        {
            Head.Disabled = PlayerQuickAccess.UPPER_BODY.Disabled;
            Body.Disabled = PlayerQuickAccess.LOWER_BODY.Disabled;
            Feet.Disabled = PlayerQuickAccess.FEET.Disabled;
        }
    }

}
