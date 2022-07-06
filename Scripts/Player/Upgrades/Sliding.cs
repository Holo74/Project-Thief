using Godot;
using System;

namespace Player.Upgrades
{
    // Adds a universal ability where the player can slide along the ground
    public class Sliding : AbstractUpgrade
    {
        private int SprintKick = 0;
        private bool CrouchChanged = false;
        public override void Applied()
        {
            // Hidden function to monitor crouch state.  This may or many not work
            Variables.CrouchChange += (state) =>
            {
                if (!state)
                {
                    CrouchChanged = false;
                }
            };
        }

        public override void Removed()
        {

        }

        public override void Update(float delta)
        {
            if (Variables.IS_SPRINTING)
            {
                // We want to have some frame buffer so that you can't go from zero to sliding.  Maybe do a speed gate?
                SprintKick = Mathf.Clamp(SprintKick + 1, 0, 11);
                if (Helper.CommonComparisions.IS_CROUCHED && !CrouchChanged)
                {
                    CrouchChanged = true;
                    Variables.MOVEMENT = new Movement.Sliding(SprintKick < 10 ? 2 : 0.8f);
                }
            }
            else
            {
                SprintKick = 0;
            }

        }
    }
}

