using Godot;
using System;

namespace NPC.Behaviour.Curious
{
    public class ObserverRotating : Behaviour.Passive.Observer
    {
        private Vector3 Forward { get; set; }
        [Export]
        private float Length { get; set; } = 0;
        private Addons.Timer Time { get; set; }
        public override void Init(NPCManager manager)
        {
            base.Init(manager);
            Forward = Manager.GlobalTransform.origin - Manager.GlobalTransform.basis.z;
            Time = new Addons.Timer(Length);
            Time.Events += () => { Manager.ReduceBehaviour(); };
        }

        public override void StartingNode()
        {
            base.StartingNode();
            Manager.TurnToLookAtGlobalPoint(Manager.GetSeenLocationInBodySpace(Manager), 1f, PlayAnimation);
            Time.ResetTime();
        }

        private void PlayAnimation()
        {
            GD.Print("Playing animation");
            Manager.Animation.Set("parameters/conditions/Investigate", true);
        }

        public override void UpdatingNode(float delta)
        {
            base.UpdatingNode(delta);
            if (Manager.DetectingPlayer == 0)
            {
                Time.UpdateTimer(delta);
            }
            else
            {
                Time.ResetTime();
            }
        }

        public override void ExitNode()
        {
            base.ExitNode();
        }
    }

}
