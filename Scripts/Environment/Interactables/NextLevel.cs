using Godot;
using System;

namespace Environment.Interactables
{
    public partial class NextLevel : AnimatableBody3D, IInteract
    {
        [Export(PropertyHint.File, "*.tscn")]
        private string Level { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public void Interact()
        {
            Management.Game.GameManager.Instance.LoadScene(Level, new Action[] { Management.Game.GameManager.Instance.SetupPlayer });
        }

        public bool CanInteract()
        {
            return true;
        }
    }

}
