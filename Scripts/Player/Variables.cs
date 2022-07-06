using Godot;
using System;

namespace Player
{
    //We must reset some values to their default states!!!
    public static class Variables
    {
        // These are the states that the player can be in
        public enum PlayerStandingState
        {
            Standing = 0,
            Crouching = 1,
            Crawling = 2
        }
        // Current state the player is in
        public static PlayerStandingState CURRENT_STANDING_STATE { get; set; }
        // Maps the current state to a speed
        public static Func<float>[] SPEED_MAPPING = { () => { return STANDING_SPEED; }, () => { return CROUCH_SPEED; }, () => { return CRAWLING_SPEED; } };

        // Resetting the variables that would be transfer when loading a save otherwise
        public static void INIT()
        {
            DEFAULT_MOVEMENT = new Movement.BasicMovement();
            CURRENT_STANDING_STATE = PlayerStandingState.Standing;
            OnFloorChange += (onFloor) => { if (onFloor) GRAVITY_MOVEMENT = Vector3.Zero; };
            isSprinting = false;
            RESET_MOVEMENT();
            RESET_ROTATION();
            #region default floats
            SPEED_MOD = 1f;
            GRAVITY_MOD = 1f;
            JUMP_MOD = 1f;
            #endregion
        }

        // The vectors that decide the movement of the player.  Should always be used as the final movement
        public static Vector3 WALKING_MOVEMENT { get; set; } = new Vector3();
        public static Vector3 GRAVITY_MOVEMENT { get; set; } = new Vector3();

        // In game, none of these should be modified by the upgrades or anything else.  For all intense and purposes they should have a private setter
        #region Static floats
        public static float GRAVITY_STRENGTH { get; set; } = 10f;
        public static float JUMP_STRENGTH { get; set; } = 5f;
        public static float STANDING_SPEED { get; set; } = 3.5f;
        public static float BOOST_WALL_JUMP { get; set; } = .2f;
        public static float SPRINT_SPEED { get; set; } = 7f;
        public static float CROUCH_SPEED { get; set; } = 1f;
        public static float CRAWLING_SPEED { get; set; } = 1f;
        public static float MOVE_TO_CROUCH { get; set; } = 5f;
        public static float MANTLE_FORWARD_SPEED { get; set; } = 6f;
        public static float MANTLE_UPWARD_SPEED { get; set; } = 7.5f;
        public static float MANTLE_UPWARD_TIME { get; set; } = 1f;
        public static float MANTLE_FORWARD_TIME { get; set; } = 0.1f;
        public static float MANTLE_BUFFER_TIMER { get; set; } = 0.1f;
        public static float ACCELERATION { get; set; } = 10f;
        public static float DECCELERATION { get; set; } = 10f;
        #endregion

        #region Modifiable floats
        public static float JUMP_MOD { get; set; } = 1f;
        public static float SPEED_MOD { get; set; } = 1f;
        public static float GRAVITY_MOD { get; set; } = 1f;
        public static float CURRENT_SPEED { get; set; } = 0f;
        #endregion

        private static bool isSprinting = false;
        public static bool IS_SPRINTING
        {
            get
            {
                return isSprinting;
            }
            set
            {
                if (value != isSprinting)
                {
                    isSprinting = value;
                    SprintingChanged?.Invoke(isSprinting);
                }
            }
        }

        // A boolean value has changed states and needs to send a signal
        public delegate void StateChange(bool newState);

        #region bool signals
        public static event StateChange SprintingChanged;
        public static event StateChange OnFloorChange;
        public static event StateChange CrouchChange;
        #endregion

        #region Rotation
        private static Rotation.BasicRotation rotation;
        public static Rotation.BasicRotation ROTATION
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public static void RESET_ROTATION()
        {
            ROTATION = new Rotation.BasicRotation();
        }
        #endregion

        #region Movement
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

        public static void RESET_MOVEMENT()
        {
            MOVEMENT = DEFAULT_MOVEMENT;
        }
        #endregion

        #region Reset 
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
        #endregion

        private static bool onFloor = true;
        public static bool ON_FLOOR
        {
            get { return onFloor; }
            set { onFloor = value; OnFloorChange?.Invoke(value); }
        }

        public static void DELETE_VARIABLES()
        {
            OnFloorChange = null;
            CrouchChange = null;
            SprintingChanged = null;
        }
    }

}
