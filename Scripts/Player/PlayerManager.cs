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
            Input.SetMouseMode(Input.MouseMode.Captured);
            Variables.INIT();
            Instance = this;
            //Variables.ADD_UPGRADE(Upgrades.AbstractUpgrade.GetUpgrade(Upgrades.AbstractUpgrade.ITEM_UPGRADE_LIST.DoubleJump));
            //Variables.ADD_UPGRADE(Upgrades.AbstractUpgrade.GetUpgrade(Upgrades.AbstractUpgrade.ITEM_UPGRADE_LIST.WallRun));
        }

        public override void _Process(float delta)
        {
            // Move this to a UI manager
            // if (Input.IsActionJustPressed("ui_cancel"))
            // {
            //     if (Input.GetMouseMode() == Input.MouseMode.Captured)
            //     {
            //         Input.SetMouseMode(Input.MouseMode.Visible);
            //         Variables.PLAYING = false;
            //     }
            //     else
            //     {
            //         Input.SetMouseMode(Input.MouseMode.Captured);
            //         Variables.PLAYING = true;
            //         Variables.RESET_ROTATION();
            //     }
            // }
            PlayerQuickAccess.INTERACTION.Interact();
        }

        //Move this into health
        // private void StandardFloorChange(bool onFloor)
        // {
        //     if (onFloor)
        //     {
        //         fallDamage(Mathf.Abs(Variables.GRAVITY_MOVEMENT.y));
        //         Variables.GRAVITY_MOVEMENT = Vector3.Zero;
        //     }
        //     else
        //     {

        //     }
        // }

        // private void StandardFallDamage(float force)
        // {
        //     if (force > 15f)
        //     {
        //         Damage(((int)(force)));
        //     }
        // }

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
    }
}