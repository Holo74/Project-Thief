using Godot;
using Player.Helper;
using System;

namespace Debug.PlayerD
{
    public partial class AreaColliders : Panel
    {
        [Export]
        private RichTextLabel Head { get; set; }
        [Export]
        private RichTextLabel Body { get; set; }
        [Export]
        private RichTextLabel LowerBody { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            Body.Text = "Closest Distance is " + StateChangeChecker.GetClosestDistance(Player.PlayerQuickAccess.SHAPE_CASTER);
            // Body.Text = "Name: " + Player.PlayerQuickAccess.SHAPE_CASTER.GetCollider(0);
        }
    }

}
