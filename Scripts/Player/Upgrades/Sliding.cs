using Godot;
using System;

namespace Player.Upgrades
{
    public class Sliding : AbstractUpgrade
    {
        private int SprintKick = 0;
        private bool CrouchChanged = false;
        public override void Applied()
        {

        }

        public override void Removed()
        {

        }

        public override void Update(float delta)
        {
            if (Variables.IS_SPRINTING)
            {
                SprintKick = Mathf.Clamp(SprintKick + 1, 0, 11);
                if (Variables.IS_CROUCHED && !CrouchChanged)
                {
                    CrouchChanged = true;
                    Variables.MOVEMENT = new Movement.Sliding(SprintKick < 10 ? 2 : 0.8f);
                }
            }
            else
            {
                SprintKick = 0;
            }
            if (!Variables.IS_CROUCHED)
            {
                CrouchChanged = false;
            }
        }
    }
}

