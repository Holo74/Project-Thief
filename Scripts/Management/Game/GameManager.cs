using Godot;
using System;

namespace Management.Game
{
    public class GameManager : Node
    {
        public delegate void StateChange(bool b);
        public static event StateChange PlayingChange;
        public static void Start()
        {
            PLAYING = true;
        }

        public static void Pause()
        {
            PLAYING = false;
        }

        private static bool playing = false;
        public static bool PLAYING
        {
            get { return playing; }
            set { playing = value; PlayingChange?.Invoke(value); }
        }
    }
}

