using Godot;
using System;

namespace Player
{
    //We must reset some values to their default states!!!
    public partial class Variables
    {

        public static Variables Instance { get; set; }
        public Handlers.CamoHandler CAMO { get; set; }
        // These are the states that the player can be in
        public enum PlayerStandingState
        {
            Standing = 0,
            Crouching = 1,
            Crawling = 2
        }
        // Current state the player is in
        public PlayerStandingState CURRENT_STANDING_STATE
        {
            get { return CurrentStandingState; }
            set
            {
                CurrentStandingState = value;
                StandingChangedTo?.Invoke(value);
            }
        }
        private PlayerStandingState CurrentStandingState { get; set; }
        public delegate void StandingStateChanged(PlayerStandingState state);
        public event StandingStateChanged StandingChangedTo;
        // Maps the current state to a speed
        // public float SPEED_MAPPING = { STANDING_SPEED, () => { return CROUCH_SPEED; }, () => { return CRAWLING_SPEED; } };
        public float SPEED_MAPPING(int input)
        {
            switch (input)
            {
                case 0:
                    return STANDING_SPEED;
                case 1:
                    return CROUCH_SPEED;
                case 2:
                    return CRAWLING_SPEED;
            }
            return 1f;
        }

        // Resetting the variables that would be transfer when loading a save otherwise
        public void INIT()
        {
            Instance = this;
            defaultMovement = new Movement.BasicMovement();
            CAMO = new Handlers.CamoHandler();
            CurrentStandingState = PlayerStandingState.Standing;
            // OnFloorChange += (onFloor) => { if (onFloor) GRAVITY_MOVEMENT = Vector3.Zero; };
            isSprinting = false;
            CAMO.Init();

        }

        // The vectors that decide the movement of the player.  Should always be used as the final movement
        public Vector3 WALKING_MOVEMENT { get; set; } = new Vector3();
        public Vector3 GRAVITY_MOVEMENT { get; set; } = new Vector3();

        // In game, none of these should be modified by the upgrades or anything else.  For all intense and purposes they should have a private setter
        #region   floats
        public float GRAVITY_STRENGTH { get; set; } = 10f;
        public float JUMP_STRENGTH { get; set; } = 5f;
        public float STANDING_SPEED { get; set; } = 3.5f;
        public float BOOST_WALL_JUMP { get; set; } = .2f;
        public float SPRINT_SPEED { get; set; } = 1.5f;
        public float CROUCH_SPEED { get; set; } = 2.2f;
        public float CRAWLING_SPEED { get; set; } = 1f;
        public float MOVE_TO_CROUCH { get; set; } = 5f;
        public float MANTLE_FORWARD_SPEED { get; set; } = 6f;
        public float MANTLE_UPWARD_SPEED { get; set; } = 7.5f;
        public float MANTLE_UPWARD_TIME { get; set; } = 1f;
        public float MANTLE_FORWARD_TIME { get; set; } = 0.1f;
        public float MANTLE_BUFFER_TIMER { get; set; } = 0.1f;
        public float ACCELERATION { get; set; } = 10f;
        public float DECCELERATION { get; set; } = 10f;
        public float MAX_CAMERA_SHAKE { get; set; } = 0.4f;
        #endregion

        #region Modifiable floats
        public float JUMP_MOD { get; set; } = 1f;
        public float SPEED_MOD { get; set; } = 1f;
        public float GRAVITY_MOD { get; set; } = 1f;
        public float CURRENT_SPEED { get; set; } = 0f;
        #endregion

        private bool isSprinting = false;
        public bool IS_SPRINTING
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

        public delegate void SimpleEventTrigger();
        public SimpleEventTrigger Jump;


        // A boolean value has changed states and needs to send a signal
        public delegate void StateChange(bool newState);

        #region bool signals
        public event StateChange SprintingChanged;
        public event StateChange OnFloorChange;
        #endregion

        #region Rotation
        private Rotation.BasicRotation rotation;
        public Rotation.BasicRotation ROTATION
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public void RESET_ROTATION()
        {
            ROTATION = new Rotation.BasicRotation();
        }
        #endregion

        #region Movement
        private Movement.AbstractMovement movement;
        public Movement.AbstractMovement MOVEMENT
        {
            get { return movement; }
            set { movement = value; movement.Starting(); }
        }
        private Movement.AbstractMovement defaultMovement;
        public Movement.AbstractMovement DEFAULT_MOVEMENT
        {
            get { return defaultMovement; }
            set { defaultMovement = value; RESET_MOVEMENT(); }
        }

        public void RESET_MOVEMENT()
        {
            MOVEMENT = defaultMovement;
        }
        #endregion

        #region Reset 
        public void RESET_JUMP_MOD()
        {
            JUMP_MOD = 1f;
        }
        public void RESET_SPEED_MOD()
        {
            SPEED_MOD = 1f;
        }
        public void RESET_GRAVITY_MOD()
        {
            GRAVITY_MOD = 1f;
        }
        #endregion

        private bool onFloor = true;
        public bool ON_FLOOR
        {
            get { return onFloor; }
            set { onFloor = value; OnFloorChange?.Invoke(value); if (value) GRAVITY_MOVEMENT = Vector3.Zero; }
        }
    }

}
