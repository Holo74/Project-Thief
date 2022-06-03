using Godot;
using System;
using Player.Movement;
using Player.Rotation;

namespace Player
{
    public class PlayerManager : KinematicBody, Interfaces.Interactions.IHealth
    {
        private delegate void FallDamage(float force);
        private FallDamage fallDamage;
        private bool onFloor = true;

        public override void _Ready()
        {
            PlayerQuickAccess.SyncVariables(this);
            fallDamage = StandardFallDamage;
            Input.SetMouseMode(Input.MouseMode.Captured);
            Variables.INIT();
            Variables.OnFloorChange += StandardFloorChange;
            //Variables.ADD_UPGRADE(Upgrades.AbstractUpgrade.GetUpgrade(Upgrades.AbstractUpgrade.ITEM_UPGRADE_LIST.DoubleJump));
            //Variables.ADD_UPGRADE(Upgrades.AbstractUpgrade.GetUpgrade(Upgrades.AbstractUpgrade.ITEM_UPGRADE_LIST.WallRun));
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                if (Input.GetMouseMode() == Input.MouseMode.Captured)
                {
                    Input.SetMouseMode(Input.MouseMode.Visible);
                    Variables.PLAYING = false;
                }
                else
                {
                    Input.SetMouseMode(Input.MouseMode.Captured);
                    Variables.PLAYING = true;
                    Variables.RESET_ROTATION();
                }
            }
            PlayerQuickAccess.INTERACTION.Interact();
        }

        private void StandardFloorChange(bool onFloor)
        {
            if (onFloor)
            {
                fallDamage(Mathf.Abs(Variables.GRAVITY_MOVEMENT.y));
                Variables.GRAVITY_MOVEMENT = Vector3.Zero;
            }
            else
            {

            }
        }

        private void StandardFallDamage(float force)
        {
            if (force > 15f)
            {
                Damage(((int)(force)));
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion mouse)
            {
                if (Variables.PLAYING)
                {
                    Variables.ROTATION?.BaseRotate(mouse);
                }
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Variables.PLAYING)
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
                foreach (Upgrades.AbstractUpgrade upgrade in Variables.UPGRADES)
                {
                    upgrade.Update(delta);
                }
            }
        }

        public void Heal(int healing)
        {
            Variables.MODIFY_HEALTH(healing);
        }

        public void Damage(int damage)
        {
            Variables.MODIFY_HEALTH(-damage);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Variables.DELETE_VARIABLES();
        }

        private void Testing(bool changed)
        {
            GD.Print(changed);
        }
    }
}