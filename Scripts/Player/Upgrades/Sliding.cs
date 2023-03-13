using Godot;
using System;

namespace Player.Upgrades
{
    // Adds a universal ability where the player can slide along the ground
    public partial class Sliding : AbstractUpgrade
    {
        private int SprintKick = 0;
        private bool CrouchChanged = false;
        public override void Applied()
        {
            // Hidden function to monitor crouch state.  This may or many not work
            Variables.Instance.StandingChangedTo += (state) =>
            {
                if (state == Variables.PlayerStandingState.Crouching)
                {
                    CrouchChanged = false;
                }
            };
        }

        public override void Removed()
        {

        }

        public override void Update(double delta)
        {
            if (Variables.Instance.IS_SPRINTING)
            {
                // We want to have some frame buffer so that you can't go from zero to sliding.  Maybe do a speed gate?
                SprintKick = Mathf.Clamp(SprintKick + 1, 0, 11);
                if (Helper.CommonComparisions.IS_CROUCHED && !CrouchChanged)
                {
                    CrouchChanged = true;
                    Variables.Instance.MOVEMENT = new Movement.Sliding(SprintKick < 10 ? 2 : 0.8f);
                }
            }
            else
            {
                SprintKick = 0;
            }

        }
    }
}

