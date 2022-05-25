using Godot;
using System;

namespace Player
{
    public static class Variables
    {
        public static void INIT()
        {
            DEFAULT_MOVEMENT = new Movement.BasicMovement();
            RESET_MOVEMENT();
            RESET_ROTATION();
        }
        public static float GRAVITY_STRENGTH { get; set; } = 10f;
        public static float JUMP_STRENGTH { get; set; } = 5f;
        public static Vector3 GRAVITY_MOVEMENT { get; set; } = new Vector3();
        public static float SPEED { get; set; } = 3.5f;
        public static Vector3 WALKING_MOVEMENT { get; set; } = new Vector3();
        public static float BOOST_WALL_JUMP { get; set; } = .2f;
        public static float SPRINT_SPEED { get; set; } = 7f;
        public static float CROUCH_SPEED { get; set; } = 1f;
        public static float CROUCHING_SPEED { get; set; } = 5f;
        public static bool IS_CROUCHED { get; set; } = false;
        public static bool IS_SPRINTING { get; set; } = false;


        public static float JUMP_MOD { get; set; } = 1f;
        public static float SPEED_MOD { get; set; } = 1f;
        public static float GRAVITY_MOD { get; set; } = 1f;

        public delegate void StateChange(bool newState);
        public static event StateChange PlayingChange;
        public static event StateChange OnFloorChange;
        private static Rotation.BasicRotation rotation;
        public static Rotation.BasicRotation ROTATION
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private static Movement.AbstractMovement movement;
        public static Movement.AbstractMovement MOVEMENT
        {
            get { return movement; }
            set { movement = value; movement.Starting(); }
        }
        private static Movement.AbstractMovement defaultMovement;
        public static Movement.AbstractMovement DEFAULT_MOVEMENT
        {
            get { return defaultMovement; }
            set { defaultMovement = value; RESET_MOVEMENT(); }
        }

        public static void RESET_ROTATION()
        {
            ROTATION = new Rotation.BasicRotation();
        }

        public static void RESET_MOVEMENT()
        {
            MOVEMENT = DEFAULT_MOVEMENT;
        }
        public static void RESET_JUMP_MOD()
        {
            JUMP_MOD = 1f;
        }
        public static void RESET_SPEED_MOD()
        {
            SPEED_MOD = 1f;
        }
        public static void RESET_GRAVITY_MOD()
        {
            GRAVITY_MOD = 1f;
        }

        private static bool playing = false;
        public static bool PLAYING
        {
            get { return playing; }
            set { playing = value; PlayingChange?.Invoke(value); }
        }
        private static bool onFloor = true;
        public static bool ON_FLOOR
        {
            get { return onFloor; }
            set { onFloor = value; OnFloorChange?.Invoke(value); }
        }

        public static void Start()
        {
            PLAYING = true;
        }

        public static void Pause()
        {
            PLAYING = false;
        }

        public static System.Collections.Generic.List<Upgrades.AbstractUpgrade> UPGRADES { get; private set; } = new System.Collections.Generic.List<Upgrades.AbstractUpgrade>();

        public static void ADD_UPGRADE(Upgrades.AbstractUpgrade upgrade)
        {
            upgrade.Applied();
            UPGRADES.Add(upgrade);
        }

        public static void REMOVE_UPGRADE(Upgrades.AbstractUpgrade upgrade)
        {
            if (UPGRADES.Contains(upgrade))
            {
                UPGRADES.Remove(upgrade);
                upgrade.Removed();
            }
        }


        public delegate void HealthChanged(int value);
        public static HealthChanged OnHealthChange;

        public static int MAX_HEALTH { get; private set; } = 100;
        public static void INCREASE_MAX_HEALTH()
        {
            MAX_HEALTH += 100;
        }
        public static int HEALTH { get; private set; } = 100;
        public static void MODIFY_HEALTH(int mod)
        {
            HEALTH = Mathf.Clamp(HEALTH + mod, -1, MAX_HEALTH);
            OnHealthChange(HEALTH);
        }

        public static void DELETE_VARIABLES()
        {
            PlayingChange = null;
            OnFloorChange = null;
            OnHealthChange = null;
        }
    }

}
