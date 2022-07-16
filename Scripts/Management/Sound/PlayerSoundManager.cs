using Godot;
using System;

namespace Management.Sound
{
    public class PlayerSoundManager : Node
    {
        private AudioStreamPlayer Legs { get; set; }
        private AudioStreamPlayer Arms { get; set; }
        public static PlayerSoundManager Instance { get; private set; }
        public delegate void SoundPlayer();
        private SoundPlayer Track { get; set; }

        public override void _Ready()
        {
            Instance = this;
            Arms = GetNode<AudioStreamPlayer>("Arms");
            Legs = GetNode<AudioStreamPlayer>("Legs");
            Track = PlayGroundWalkingSound;
            Player.Variables.OnFloorChange += (state) => { if (state) { Track = PlayGroundWalkingSound; } else { Track = PlayFlyingSound; } };
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            Track();
        }

        private void PlayFlyingSound()
        {

        }

        private void PlayGroundWalkingSound()
        {
            if (!Legs.Playing && Player.Variables.WALKING_MOVEMENT.LengthSquared() > 0f)
            {
                PlayLegSound(Player.Variables.CAMO.GetSound(), loudness: Player.Variables.CAMO.GetSoundVolume());
            }
        }

        public void PlayLegSound(AudioStream sound, bool overrideSound = false, float loudness = 1f)
        {
            if (!Legs.Playing || overrideSound)
            {
                Legs.Stream = sound;
                Legs.VolumeDb = (loudness * 20) - 20f;
                Legs.Play();
            }
        }

        public void PlayArmSound(AudioStream sound)
        {
            Arms.Stream = sound;
            Arms.Play();
        }
    }

}
