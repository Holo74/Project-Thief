using Godot;
using System;

namespace Player.Interactions
{
    public abstract class AbstractInteraction
    {
        public virtual void SetInteraction()
        {

        }
        public abstract bool CanInteract();

        public abstract void Interacted();

        private static AbstractInteraction[] InteractionList =
        {
            new BasicInteraction(),
            new PickedUp()
        };

        public enum InteractionTypes
        {
            Basic,
            Pickup
        }

        public static AbstractInteraction GET_INTERACTION(InteractionTypes type)
        {
            return InteractionList[((int)type)];
        }
    }

}
