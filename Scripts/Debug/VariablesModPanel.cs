using Godot;
using System;

namespace Debug
{
    public class VariablesModPanel : Panel
    {
        [Export]
        private bool Enabled { get; set; }
        public override void _Ready()
        {
            Visible = false;
            if (Enabled)
            {
                Management.Game.GameManager.PlayingChange += (playing) => { Visible = !playing; };
            }
        }
    }

}
