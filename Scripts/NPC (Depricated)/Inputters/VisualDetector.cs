using Godot;
using System;

namespace NPC.Inputters
{
    public class VisualDetector : SignalEmitter
    {
        [Export]
        private float CuriousDetectionLimit { get; set; } = 0;
        [Export]
        private float InvestigatingDetectionLimit { get; set; } = 0;
        [Export]
        private float SeenDetectionLimit { get; set; }
        private float CurrentDetection { get; set; } = 0;
        private Spatial[] DetectionPoints { get; set; }
        private RayCast[] RayCasts { get; set; }
        private float PlayerDetectionRate { get; set; } = 0;
        [Export]
        private float InDirectSight { get; set; }
        [Export]
        private float InSideSight { get; set; }
        private int SightMultiplier = 0;
        private float TotalRayDistance { get; set; }
        private bool SeenLastFrame { get; set; }

        public override void _Ready()
        {
            SeenLastFrame = false;
            RayCasts = new RayCast[3];
            RayCasts[0] = GetNode<RayCast>("RayCast");
            RayCasts[1] = GetNode<RayCast>("RayCast2");
            RayCasts[2] = GetNode<RayCast>("RayCast3");
            TotalRayDistance = RayCasts[0].CastTo.Length();
        }

        public override void _Process(float delta)
        {
            float previous = CurrentDetection;
            if (SightMultiplier > 0)
            {
                CurrentDetection += delta * TotalViewable() * (SightMultiplier > 1 ? InSideSight : InDirectSight);
                if (CurrentDetection > SeenDetectionLimit)
                {
                    EmitSignal(nameof(SwitchState), NPCManager.BehaviourState.Alert);
                }
                else
                {
                    if (CurrentDetection > InvestigatingDetectionLimit)
                    {
                        EmitSignal(nameof(SwitchState), NPCManager.BehaviourState.Investigating);
                    }
                    else
                    {
                        if (CurrentDetection > CuriousDetectionLimit)
                        {
                            EmitSignal(nameof(SwitchState), NPCManager.BehaviourState.Curious);
                        }
                    }
                }
                EmitSignal(nameof(CreateMemory), Player.PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin, Player.PlayerManager.Instance, MemoryType.Visual);
            }
        }

        private int TotalViewable()
        {
            int Viewable = 0;
            Viewable += RayCastToPlayer(RayCasts[0], 0);
            Viewable += RayCastToPlayer(RayCasts[1], 1);
            Viewable += RayCastToPlayer(RayCasts[2], 2);
            if ((Viewable > 0) != SeenLastFrame)
            {
                SeenLastFrame = (Viewable > 0);
                EmitSignal(nameof(SeenCounter), SeenLastFrame ? 1 : -1);
            }
            return Viewable;
        }

        public void BehaviourSetTo(NPCManager.BehaviourState state)
        {
            switch (state)
            {
                case NPCManager.BehaviourState.Passive:
                    CurrentDetection = 0;
                    break;
                case NPCManager.BehaviourState.Curious:
                    CurrentDetection = CuriousDetectionLimit;
                    break;
                case NPCManager.BehaviourState.Investigating:
                    CurrentDetection = InvestigatingDetectionLimit;
                    break;
                case NPCManager.BehaviourState.Alert:
                    CurrentDetection = SeenDetectionLimit;
                    break;
                case NPCManager.BehaviourState.Dead:
                    break;
            }
        }

        public int RayCastToPlayer(RayCast ray, int body)
        {
            ray.CastTo = Vector3.Forward * TotalRayDistance * (Player.PlayerManager.Instance.GetStealthValue() / 1.5f);
            ray.LookAt(DetectionPoints[body].GlobalTranslation, Vector3.Up);
            return ray.GetCollider() is Player.BodyMods.DetectionPoints ? 1 : 0;
        }

        private void AssignDetectionPoints()
        {
            if (DetectionPoints is null)
            {
                DetectionPoints = new Spatial[3];
                Godot.Collections.Array a = GetTree().GetNodesInGroup("DetectionPoint");
                for (int i = 0; i < 3; i++)
                {
                    DetectionPoints[i] = (Spatial)a[i];
                }
            }
        }

        private void BodyEnterDirectSight(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                AssignDetectionPoints();
                SightMultiplier += 2;
            }
        }

        private void BodyExitDirectSight(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                SightMultiplier -= 2;
            }
        }

        private void BodyEnterIndirectSight(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                AssignDetectionPoints();
                SightMultiplier += 1;
            }
        }

        private void BodyExitIndirectSight(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                SightMultiplier -= 1;
            }
        }
    }

}
