using Godot;
using System;

namespace Management.Time
{
    public partial class StartPlayerTimer : Timer
    {
        public override void _Ready()
        {
            Connect("timeout",new Callable(this,nameof(StartPlayer)));
        }

        private void StartPlayer()
        {
            Management.Game.GameManager.Start();
            // GD.Print("Game Start");
        }
    }

}
