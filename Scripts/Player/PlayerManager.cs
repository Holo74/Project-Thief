using Godot;
using System;
using Player.Movement;
using Player.Rotation;

namespace Player
{
    public partial class PlayerManager : CharacterBody3D, Interfaces.Interactions.IHealth
    {
        [Export]
        public Handlers.Health PlayerHealth { get; set; }
        [Export]
        public Handlers.UpgradeHandler Upgrades { get; set; }

        public static PlayerManager Instance { get; private set; }

        public override void _Ready()
        {
            PlayerQuickAccess.SyncVariables(this);
            //Input.SetMouseMode(Input.MouseMode.Captured);
            Instance = this;
            // GD.Print(GetViewport().ShadowAtlasSize);
            ConnectVariablesToPlayer();
        }

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed("ToggleThirdPerson"))
            {
                if (PlayerQuickAccess.CAMERA.Current)
                {
                    PlayerQuickAccess.CAMERA.Current = false;
                    GetNode<Camera3D>("Camera3D").Current = true;
                }
                else
                {
                    PlayerQuickAccess.CAMERA.Current = true;
                    GetNode<Camera3D>("Camera3D").Current = false;

                }
            }
            if (Management.Game.GameManager.PLAYING)
            {
                PlayerQuickAccess.INTERACTION.Interact();
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion mouse)
            {
                if (Management.Game.GameManager.PLAYING)
                {
                    Variables.Instance.ROTATION?.BaseRotate(mouse);
                }
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Management.Game.GameManager.PLAYING)
            {
                if (Variables.Instance.ON_FLOOR)
                {
                    Variables.Instance.MOVEMENT.Movement(delta);
                }
                else
                {
                    Variables.Instance.MOVEMENT.FallingMovement(delta);
                }
                Variables.Instance.MOVEMENT.FloorDetection();
                Upgrades.RunUpgrades(delta);
            }
        }

        public void ReceiveHealthUpdate(Player.Handlers.Health.InteractionTypes type, int amount)
        {
            PlayerHealth.ModifyHealth(type, amount);
            if (PlayerHealth.CurrentHealth <= 0)
            {
                Management.Game.GameManager.Instance.QuitToMainMenu();
            }
        }

        public float GetStealthValue()
        {
            return Helper.MathEquations.GET_STEALTH_VALUE(PlayerQuickAccess.LIGHT.CurrentLight, Variables.Instance.CAMO.BaseVisibility);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void ConnectVariablesToPlayer()
        {
            Variables.Instance.DEFAULT_MOVEMENT = new BasicMovement();
            Variables.Instance.OnFloorChange += (state) => { if (state) ReceiveHealthUpdate(Handlers.Health.InteractionTypes.Falling, -(int)Math.Pow(Mathf.Clamp(Mathf.Abs(Variables.Instance.GRAVITY_MOVEMENT.Y) - 10, 0, Mathf.Inf), 3)); };
        }
    }
}
