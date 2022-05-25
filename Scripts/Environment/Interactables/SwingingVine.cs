using Godot;
using System;

namespace Environment.Interactables
{
    public class SwingingVine : Node, IInteract
    {
        public bool CanInteract()
        {
            return false;
        }

        public void Interact()
        {

        }
    }

}
