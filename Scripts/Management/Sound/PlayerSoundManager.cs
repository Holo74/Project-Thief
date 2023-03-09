using Godot;
using System;

namespace Management.Sound
{
    public partial class PlayerSoundManager : Node
    {
        private AudioStreamPlayer Legs { get; set; }
        private AudioStreamPlayer Arms { get; set; }
        public static PlayerSoundManager Instance { get; private set; }
        public delegate void SoundPlayer();
        private SoundPlayer Track { get; set; }
        public float SoundLevel { get; set; }
        private float SettingSoundLevel { get; set; }
        [Export]
        private AudioStream Jump { get; set; }
        private int[] LandingMods = { 1, 2, 4 }; // Standing, crouch, crawling

        public override void _Ready()
        {
            Instance = this;
            Arms = GetNode<AudioStreamPlayer>("Arms");
            Legs = GetNode<AudioStreamPlayer>("Legs");
            Track = PlayGroundWalkingSound;
            Player.Variables.Instance.OnFloorChange += FloorChange;
            Player.Variables.Instance.Jump += PlayJump;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            SettingSoundLevel = 0;
            Track();
            SoundLevel += SettingSoundLevel * 10f;
        }

        private void FloorChange(bool state)
        {
            if (state)
            {
                Track = PlayGroundWalkingSound;
                float loudness = Player.Variables.Instance.CAMO.GetSoundVolume() / LandingMods[(int)Player.Variables.Instance.CURRENT_STANDING_STATE] * Player.Variables.Instance.GRAVITY_MOVEMENT.Y;
                PlayLegSound(Player.Variables.Instance.CAMO?.GetSoundDictionary()?.LandingSound, true, Mathf.Clamp(loudness, 0f, 1f));
            }
            else
            {
                Track = PlayFlyingSound;
            }
        }

        private void PlayFlyingSound()
        {

        }

        private void PlayJump()
        {
            SoundLevel += Player.Variables.Instance.GRAVITY_MOVEMENT.Y;
            PlayLegSound(Jump, true);
        }

        private void PlayGroundWalkingSound()
        {
            SettingSoundLevel += Player.Variables.Instance.WALKING_MOVEMENT.Length();
            if (!Legs.Playing && SettingSoundLevel > 0f)
            {
                PlayLegSound(Player.Variables.Instance.CAMO.GetSound(), loudness: Player.Variables.Instance.CAMO.GetSoundVolume());
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
