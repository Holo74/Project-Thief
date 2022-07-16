using Godot;
using System;

namespace Player.Helper
{
    // Used as a place where all math equations go so we can inspect their inner workings.  Don't put simple math here
    public static class MathEquations
    {
        public static float GET_SPEED()
        {
            return Variables.SPEED_MAPPING[(int)Variables.CURRENT_STANDING_STATE]() * (Variables.IS_SPRINTING ? Variables.SPRINT_SPEED : 1) * Variables.SPEED_MOD;
        }

        public static float GET_STEALTH_VALUE(float light, int camo)
        {
            return ((100 - camo) * (light + 0.1f)) / 100f;
        }
    }

}
