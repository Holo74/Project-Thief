using Godot;
using System;

namespace NPC
{
    public class NPCManager : RigidBody
    {
        [Export]
        private Behaviour.BehaviourBase Passive { get; set; }
        [Export]
        private Behaviour.BehaviourBase Curious { get; set; }
        [Export]
        private Behaviour.BehaviourBase Investigating { get; set; }
        [Export]
        private Behaviour.BehaviourBase Alert { get; set; }
        private Behaviour.BehaviourBase Current { get; set; }
        public Memories CurrentMemory { get; private set; }
        public BehaviourState CurrentState { get; private set; } = BehaviourState.UnInit;
        public int PlayerFoundCounter { get; set; }
        public NavigationAgent Navigator { get; set; }
        [Export]
        private AudioStreamSample[] PassiveSounds { get; set; }
        [Export]
        private AudioStreamSample[] CuriousSounds { get; set; }
        [Export]
        private AudioStreamSample[] InvestigationSounds { get; set; }
        [Export]
        private AudioStreamSample[] AlertSounds { get; set; }
        public delegate void SafeVelocity(Vector3 velocity);
        public event SafeVelocity SVelocity;
        public delegate void DestinationReached();
        public event DestinationReached Reached;
        public AudioStreamPlayer3D Speaking { get; set; }
        public AudioStreamPlayer3D Feet { get; set; }
        public int DetectingPlayer { get; set; }
        private MeshInstance Indicator { get; set; }
        [Signal]
        private delegate void BehaviourChanged(BehaviourState state);
        public AnimationTree Animation { get; private set; }

        public override void _Ready()
        {
            DetectingPlayer = 0;
            Navigator = GetNode<NavigationAgent>("NavigationAgent");
            Speaking = GetNode<AudioStreamPlayer3D>("Mouth");
            Feet = GetNode<AudioStreamPlayer3D>("Feet");
            Indicator = GetNode<MeshInstance>("Indicator");
            Animation = GetNode<AnimationTree>("AnimationTree");
            Passive.Init(this);
            Curious.Init(this);
            Investigating.Init(this);
            Alert.Init(this);
            SetBehaviour(BehaviourState.Passive);
            CurrentMemory = new Memories();
        }

        public bool IsDetectingPlayer()
        {
            return DetectingPlayer > 0;
        }

        public override void _Process(float delta)
        {
            if (!(Player.PlayerManager.Instance is null))
            {
                Vector3 direction = Player.PlayerQuickAccess.KINEMATIC_BODY.GlobalTransform.origin - Indicator.GlobalTransform.origin;
                Indicator.LookAt(Indicator.GlobalTransform.origin - direction.Normalized(), Vector3.Up);
            }
            Current?.UpdatingNode(delta);
        }

        public override void _PhysicsProcess(float delta)
        {
            Current?.UpdatingPhysicsNode(delta);
        }

        public void CreateMemory(Vector3 position, Node source, MemoryType type)
        {
            CurrentMemory.CreateMemory(type, position, source);
        }

        public void SetBehaviour(BehaviourState state)
        {
            if ((int)CurrentState >= (int)state)
            {
                return;
            }
            SetStateFromBehaviour(state);
            CurrentState = state;
        }

        private void ReachedTarget()
        {
            Reached?.Invoke();
        }

        private void SafeVelocityCalc(Vector3 velo)
        {
            if (FinishRotating)
            {
                LinearVelocity = velo;
            }
            SVelocity?.Invoke(velo);

        }

        public void ReduceBehaviour()
        {
            CurrentState = (BehaviourState)(((int)CurrentState) - 1);
            if (!CurrentMemory.IsMemoryResolved())
            {
                CurrentMemory.ResolveCurrentMemory();
            }
            SetStateFromBehaviour(CurrentState);
        }

        private void ChangeSeenCounter(int changeBy)
        {
            DetectingPlayer += changeBy;
        }

        private void SetStateFromBehaviour(BehaviourState state)
        {
            Current?.ExitNode();
            GD.Print("Moving from: " + CurrentState + " To: " + state);
            float value = 0;
            switch (state)
            {
                case BehaviourState.Passive:
                    Current = Passive;
                    PlayRandomSound(PassiveSounds, Speaking);
                    break;
                case BehaviourState.Curious:
                    Current = Curious;
                    PlayRandomSound(CuriousSounds, Speaking);
                    value = 0.33f;
                    break;
                case BehaviourState.Investigating:
                    PlayRandomSound(InvestigationSounds, Speaking);
                    Current = Investigating;
                    value = 0.66f;
                    break;
                case BehaviourState.Alert:
                    PlayRandomSound(AlertSounds, Speaking);
                    value = 1.0f;
                    Current = Alert;
                    break;
            }
            CreateTween().TweenProperty(Indicator.GetActiveMaterial(0), "shader_param/Visibility_Rating", value, 0.2f);
            Current.StartingNode();
            EmitSignal(nameof(BehaviourChanged), state);
        }

        private void PlayRandomSound(AudioStreamSample[] samples, AudioStreamPlayer3D position, bool force = false)
        {
            if (force || !position.Playing)
            {
                int random = Management.Game.GameManager.Instance.Generator.RandiRange(0, samples.Length - 1);
                AudioStreamSample sample = samples[random];
                position.Stream = sample;
                position.Play();
            }
        }

        public bool CanSeePlayer()
        {
            return DetectingPlayer > 0;
        }

        public Vector3 GetSeenLocationInBodySpace(Spatial space)
        {
            return space.ToLocal(GetSeenLocation());
        }

        public Vector3 GetSeenLocation()
        {
            Vector3 location = GlobalTransform.origin;
            if (CanSeePlayer())
            {
                location = Player.PlayerManager.Instance.GlobalTransform.origin;
            }
            else
            {
                if (!CurrentMemory.IsMemoryResolved())
                {
                    location = CurrentMemory.CurrentMemory.Position;
                }
            }
            return location;
        }

        public void MoveToNextLocation(float speed)
        {
            Vector3 destination = (Navigator.GetNextLocation() - GlobalTransform.origin).Normalized();
            Navigator.SetVelocity(destination * speed);
            GlobalRotate(Vector3.Up, GlobalTransform.basis.GetEuler().AngleTo(destination) * GetPhysicsProcessDeltaTime() * speed);
        }

        public void TurnToLookAtGlobalPoint(Vector3 point, float time = 0.2f, Action action = null)
        {
            if (FinishRotating)
            {
                point.y = GlobalTransform.origin.y;
                if (!GlobalTransform.LookingAt(point, Vector3.Up).IsEqualApprox(GlobalTransform))
                {
                    EndRot = GlobalTransform.LookingAt(point, Vector3.Up);
                    SceneTreeTween t = CreateTween();
                    t.SetProcessMode(Tween.TweenProcessMode.Physics).TweenMethod(this, nameof(TweenRotate), GlobalRotation, EndRot.basis.GetEuler(), time);
                    PlayAfterRotation = action;
                    t.TweenCallback(this, nameof(FinishRotation));
                    FinishRotating = false;
                }
            }
        }
        private Transform EndRot { get; set; }
        private Action PlayAfterRotation { get; set; }
        private bool FinishRotating { get; set; } = true;
        private void TweenRotate(Vector3 rot)
        {
            GlobalRotation = rot;
        }

        private void FinishRotation()
        {
            FinishRotating = true;
            PlayAfterRotation?.Invoke();
        }

        public enum BehaviourState
        {
            UnInit = -1,
            Passive,
            Curious,
            Investigating,
            Alert,
            Dead
        }
    }

}
