using Godot;
using System;

namespace Player.Interactions
{
    public partial class BasicInteraction : AbstractInteraction
    {
        public override bool CanInteract()
        {
            if (PlayerQuickAccess.INTERACTION.IsColliding())
            {
                if (PlayerQuickAccess.INTERACTION.GetCollider() is Environment.Interactables.IInteract e)
                {
                    return e.CanInteract();
                }
            }
            return false;
        }

        public override void Interacted()
        {
            ((Environment.Interactables.IInteract)PlayerQuickAccess.INTERACTION.GetCollider()).Interact();
        }

        public override void SetInteraction()
        {
            PlayerQuickAccess.INTERACTION.CollisionMask = 1;
        }
    }

}
