using Godot;
using System;

namespace Player.GUI
{
    public class InteractionTexture : TextureRect
    {
        public override void _Process(float delta)
        {
            Visible = Player.PlayerQuickAccess.INTERACTION.InteractMod.CanInteract();
        }
    }

}
