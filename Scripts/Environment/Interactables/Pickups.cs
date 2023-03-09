using Godot;
using System;
using Player;
using Player.Interactions;

namespace Environment.Interactables
{
    public partial class Pickups : RigidBody3D, IInteract
    {
        public void Interact()
        {
            GD.PrintErr("This is an invalid interaction");
            return;

        }

        public bool CanInteract()
        {
            return false;
        }
    }

}
