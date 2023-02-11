using Godot;
using System;

namespace NPC.Addons
{
    public class Timer
    {
        public Timer(float time)
        {
            Interval = time;
        }
        private float Interval { get; set; }
        private float CurrentTime { get; set; }
        public delegate void FireEvent();
        public event FireEvent Events;

        public void ResetTime()
        {
            CurrentTime = 0;
        }

        public void UpdateTimer(float delta)
        {
            CurrentTime += delta;
            if (CurrentTime > Interval)
            {
                Events?.Invoke();
                CurrentTime = 0;
            }
        }
    }

}
