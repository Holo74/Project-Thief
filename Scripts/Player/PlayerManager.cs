using Godot;
using System;
using Player.Movement;
using Player.Rotation;

namespace Player
{
    public class PlayerManager : KinematicBody, Interfaces.Interactions.IHealth
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
            Variables.DEFAULT_MOVEMENT = new BasicMovement();
            Variables.OnFloorChange += (state) => { if (state) ReceiveHealthUpdate(Handlers.Health.InteractionTypes.Falling, -(int)Math.Pow(Mathf.Clamp(Mathf.Abs(Variables.GRAVITY_MOVEMENT.y) - 10, 0, Mathf.Inf), 3)); };
            // GD.Print(GetViewport().ShadowAtlasSize);
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("ToggleThirdPerson"))
            {
                if (PlayerQuickAccess.CAMERA.Current)
                {
                    PlayerQuickAccess.CAMERA.Current = false;
                    GetNode<Camera>("ClippedCamera").Current = true;
                }
                else
                {
                    PlayerQuickAccess.CAMERA.Current = true;
                    GetNode<Camera>("ClippedCamera").Current = false;

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
                    Variables.ROTATION?.BaseRotate(mouse);
                }
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Management.Game.GameManager.PLAYING)
            {
                if (Variables.ON_FLOOR)
                {
                    Variables.MOVEMENT.Movement(delta);
                }
                else
                {
                    Variables.MOVEMENT.FallingMovement(delta);
                }
                Variables.MOVEMENT.FloorDetection();
                Upgrades.RunUpgrades(delta);
            }
        }

        public void ReceiveHealthUpdate(Player.Handlers.Health.InteractionTypes type, int amount)
        {
            PlayerHealth.ModifyHealth(type, amount);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Variables.DELETE_VARIABLES();
        }

        public float GetStealthValue()
        {
            return Helper.MathEquations.GET_STEALTH_VALUE(PlayerQuickAccess.LIGHT.CurrentLight, Variables.CAMO.BaseVisibility);
        }
    }
}