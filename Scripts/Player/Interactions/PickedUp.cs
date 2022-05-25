using Godot;
using System;

namespace Player.Interactions
{
    public class PickedUp : AbstractInteraction
    {
        public override bool CanInteract()
        {
            return true;
        }

        public override void Interacted()
        {
            PlayerQuickAccess.INTERACTION.GetChild<Environment.Interactables.Pickups>(0).Interact();
        }

        public override void SetInteraction()
        {
            PlayerQuickAccess.INTERACTION.CollisionMask = 32768;
        }
    }
}

