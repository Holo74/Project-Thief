using Godot;
using System;

namespace Management.Sound
{
    public class PlayerSoundManager : Node
    {
        private AudioStreamPlayer Legs { get; set; }
        private AudioStreamPlayer Arms { get; set; }
        public static PlayerSoundManager Instance { get; private set; }
        public override void _Ready()
        {
            Instance = this;
            Arms = GetNode<AudioStreamPlayer>("Arms");
            Legs = GetNode<AudioStreamPlayer>("Legs");
        }

        public void PlayLegSound(AudioStream sound)
        {
            Legs.Stream = sound;
            Legs.Play();
        }

        public void PlayArmSound(AudioStream sound)
        {
            Arms.Stream = sound;
            Arms.Play();
        }
    }

}
