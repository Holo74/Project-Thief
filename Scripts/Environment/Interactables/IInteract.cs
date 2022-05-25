using Godot;
using System;

namespace Environment.Interactables
{
    public interface IInteract
    {
        void Interact();
        bool CanInteract();
    }
}

