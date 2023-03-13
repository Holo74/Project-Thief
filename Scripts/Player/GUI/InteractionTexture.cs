using Godot;
using System;

namespace Player.GUI
{
    public partial class InteractionTexture : TextureRect
    {
        public override void _Process(double delta)
        {
            Visible = Player.PlayerQuickAccess.INTERACTION.InteractMod.CanInteract();
        }
    }

}
