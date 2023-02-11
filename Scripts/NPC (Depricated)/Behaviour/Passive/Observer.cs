using Godot;
using System;

namespace NPC.Behaviour.Passive
{
    public class Observer : BehaviourBase
    {
        private Vector3 Starting { get; set; }
        private bool MoveToStart { get; set; }
        public override void Init(NPCManager manager)
        {
            base.Init(manager);
            Starting = NavigationServer.MapGetClosestPoint(Manager.Navigator.GetNavigationMap(), Manager.GlobalTransform.origin);
        }

        public override void StartingNode()
        {
            base.StartingNode();
            if (Starting.DistanceSquaredTo(Manager.GlobalTransform.origin) > 1f)
            {
                MoveToStart = true;
                Manager.Navigator.SetTargetLocation(Starting);
            }
        }

        public override void UpdatingPhysicsNode(float delta)
        {
            if (MoveToStart)
            {
                Manager.Navigator.GetNextLocation();
            }
        }

        public override void UpdatingNode(float delta)
        {

        }

        public override void ExitNode()
        {

        }
    }

}
