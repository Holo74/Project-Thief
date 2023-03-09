using Godot;
using System;

namespace Player.Interactions
{
    public partial class Interaction : RayCast3D
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
