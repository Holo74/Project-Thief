using Godot;
using System;

namespace Player.Helper
{
    public static class CommonComparisions
    {
        public static bool IS_CROUCHED
        {
            get
            {
                return Variables.CURRENT_STANDING_STATE == Variables.PlayerStandingState.Crouching;
            }
        }
    }

}
