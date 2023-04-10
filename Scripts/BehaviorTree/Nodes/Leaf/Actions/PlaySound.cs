using Godot;
using System;

namespace BehaviorTree.Nodes.Leaf.Actions
{
    public partial class PlaySound : Base
    {
        [Export]
        private bool MouthSound { get; set; }
        [Export]
        private AudioStream Audio { get; set; }
        public override Results Tick(double delta, BehaviorController BC)
        {
            base.Tick(delta, BC);
            if (MouthSound)
            {
                BC.MouthSoundPlayer.Stream = Audio;
                BC.MouthSoundPlayer.Play();
            }
            else
            {
                BC.FeetSoundPlayer.Stream = Audio;
                BC.FeetSoundPlayer.Play();
            }
            BC.BlackBoard[Enums.KeyList.Debugging] = "Playing Soundd";
            return Results.Success;
        }
    }

}
