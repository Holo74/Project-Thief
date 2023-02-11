using Godot;
using System;

namespace NPC.Behaviour.Alert
{
    public class MoveAndMelee : BehaviourBase
    {
        [Export]
        private float TimerLength { get; set; } = 10f;
        private Addons.Timer ResetBeh { get; set; }
        private Action<float> CurrentState { get; set; }
        private float Speed { get; set; } = 1f;
        public override void Init(NPCManager manager)
        {
            base.Init(manager);
            ResetBeh = new Addons.Timer(TimerLength);
        }

        private void ReachTarget()
        {
            if (Manager.DetectingPlayer > 0)
            {
                CurrentState = Attack;
            }
            else
            {
                CurrentState = Investigate;
            }
        }

        private void SafeMove(Vector3 move)
        {
            Manager.LinearVelocity = move;
        }

        public override void StartingNode()
        {
            base.StartingNode();
            CurrentState = Chase;
        }

        public override void ExitNode()
        {

        }

        public override void UpdatingNode(float delta)
        {

        }

        public override void UpdatingPhysicsNode(float delta)
        {
            CurrentState(delta);
        }

        private void TransitionToChase()
        {
            if ((Manager.DetectingPlayer > 0 || !Manager.CurrentMemory.IsMemoryResolved()) && Manager.Navigator.DistanceToTarget() > 2.0f)
            {
                CurrentState = Chase;
            }
        }

        public void Attack(float delta)
        {

            TransitionToChase();
        }

        public void Investigate(float delta)
        {

            TransitionToChase();
        }

        public void Chase(float delta)
        {
            if (Manager.DetectingPlayer > 0 || !Manager.CurrentMemory.IsMemoryResolved())
            {
                GD.Print("Chasing");
                Manager.Navigator.SetTargetLocation(Manager.GetSeenLocationInBodySpace(Manager));
                Manager.MoveToNextLocation(Speed);
            }
        }

        public void InvestigateSurroundings()
        {

        }
    }

}
