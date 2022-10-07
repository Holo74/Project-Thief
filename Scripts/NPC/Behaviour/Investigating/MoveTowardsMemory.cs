using Godot;
using System;

namespace NPC.Behaviour.Investigating
{
    public class MoveTowardsMemory : BehaviourBase
    {
        [Export]
        private float Speed { get; set; } = 0;
        private bool Investigating { get; set; } = false;
        [Export]
        private float InvestigateLimit { get; set; }
        private Addons.Timer Time { get; set; }

        public override void Init(NPCManager manager)
        {
            base.Init(manager);
            Time = new Addons.Timer(InvestigateLimit);
            Time.Events += () => { Manager.ReduceBehaviour(); };
        }

        public override void StartingNode()
        {
            base.StartingNode();
            Investigating = false;
            Manager.Navigator.SetTargetLocation(NavigationServer.MapGetClosestPoint(Manager.Navigator.GetNavigationMap(), Manager.GetSeenLocation()));
            Manager.Reached += Investigate;
            Time.ResetTime();
        }

        private void Investigate()
        {
            Investigating = true;
        }

        public override void ExitNode()
        {
            Manager.Reached -= Investigate;
            Investigating = false;
        }

        public override void UpdatingNode(float delta)
        {
            if (Investigating)
            {
                Time.UpdateTimer(delta);
            }
        }

        public override void UpdatingPhysicsNode(float delta)
        {
            if (!Investigating)
            {
                Manager.MoveToNextLocation(Speed);
            }
        }
    }

}
