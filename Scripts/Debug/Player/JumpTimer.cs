using Godot;
using System;
using System.Diagnostics;

namespace Debug.PlayerD
{
    public class JumpTimer : Node
    {
        private long InitialJump { get; set; }
        private long AfterJump { get; set; }
        private Stopwatch Watch { get; set; }
        public override void _Ready()
        {
            Watch = new Stopwatch();
            Player.Variables.Instance.OnFloorChange += FloorChange;
        }

        private void FloorChange(bool onFloor)
        {
            if (onFloor)
            {
                Watch.Stop();
                GD.Print(Watch.ElapsedMilliseconds * 0.001f);
            }
            else
            {
                Watch.Restart();
            }
        }
    }

}
