using Godot;
using System;

namespace NPC.Inputters
{
    public class SoundListener : SignalEmitter
    {
        [Export]
        private float CuriousLimit { get; set; }
        [Export]
        private float InvesitigatingLimit { get; set; }

        public delegate void EmitSound(Vector3 position, float soundLevel, Node source);
        public static EmitSound Listeners;
        public override void _Ready()
        {
            AddToGroup("Listener");
            Listeners += SoundHerder;
        }

        public void SoundHerder(Vector3 position, float soundLevel, Node source)
        {
            float intensity = soundLevel / position.DistanceSquaredTo(GlobalTransform.origin);
            if (CuriousLimit < intensity)
            {
                GD.Print("Heard something");
                if (InvesitigatingLimit < intensity)
                {
                    EmitSignal(nameof(SwitchState), NPCManager.BehaviourState.Investigating);
                }
                else
                {
                    EmitSignal(nameof(SwitchState), NPCManager.BehaviourState.Curious);
                }
                EmitSignal(nameof(CreateMemory), position, source, MemoryType.Audio);
            }
        }
    }

}
