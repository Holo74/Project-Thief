using Godot;
using System;

public partial class WinInteract : StaticBody3D, Environment.Interactables.IInteract
{
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Management.Game.GameManager.Instance.QuitToMainMenu();
    }
}
