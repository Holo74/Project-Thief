using Godot;
using System;

namespace Environment.Resources
{
    public class SoundDictionary : Resource
    {
        [Export]
        public AudioStream[] Sounds { get; private set; }
        // From 0 to 1
        [Export(PropertyHint.Range, "0, 1")]
        public float Loudness { get; private set; }
        [Export]
        public AudioStream LandingSound { get; private set; }

        public AudioStream GetRandomSound()
        {
            RandomNumberGenerator random = new RandomNumberGenerator();
            random.Seed = (ulong)System.DateTime.Now.Ticks;
            int range = random.RandiRange(0, Sounds.Length - 1);
            // GD.Print(range + " Range of sounds");
            return Sounds[range];
        }
    }
}

