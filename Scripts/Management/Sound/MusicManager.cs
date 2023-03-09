using Godot;
using System;

namespace Management.Sound
{
    public partial class MusicManager : Node
    {
        private AudioStreamPlayer Ambience { get; set; }
        private AudioStreamPlayer Music { get; set; }
        public static MusicManager Instance { get; private set; }
        public override void _Ready()
        {
            Instance = this;
            Ambience = GetNode<AudioStreamPlayer>("Ambient Sound");
            Music = GetNode<AudioStreamPlayer>("Music");
        }

        public void SetAmbience(AudioStream ambience)
        {
            Ambience.Stream = ambience;
            Music.Playing = false;
            Ambience.Play();
        }

        public void SetMusic(AudioStream music)
        {
            Music.Stream = music;
            Ambience.Playing = false;
            Music.Play();
        }

        public void Pause()
        {
            Ambience.Playing = false;
            Music.Playing = false;
        }
    }

}
