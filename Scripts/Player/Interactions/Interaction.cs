using Godot;
using System;

namespace Player.Interactions
{
    public class Interaction : RayCast
    {
        public AbstractInteraction InteractMod { get; private set; }
        public void Interact()
        {
            if (Input.IsActionJustPressed("Interaction"))
            {
                if (InteractMod.CanInteract())
                {
                    InteractMod.Interacted();
                }
            }
        }

        public void SetInteraction(AbstractInteraction interact)
        {
            InteractMod = interact;
            InteractMod.SetInteraction();
        }
    }

}
