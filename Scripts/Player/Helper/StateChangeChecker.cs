using Godot;
using System;

namespace Player.Helper
{
    public static class StateChangeChecker
    {
        // Used to reference whether they are able to change states.  The row is the current state and the column is the state they are going to
        private static Func<bool>[,] StateMatix =
        {
            {() => {return true;}, () => {return true;}, () => {return true;}},
            {() => {return GetClosestDistance(PlayerQuickAccess.SHAPE_CASTER) > 1.5f;}, () => {return true;}, () => {return true;}},
            // {() => {return PlayerQuickAccess.UPPER_BODY_AREA.GetOverlappingBodies().Count == 0;}, () => {return true;}, () => {return true;}},
            {() => {return GetClosestDistance(PlayerQuickAccess.SHAPE_CASTER) > 1.5f;}, () => { return GetClosestDistance(PlayerQuickAccess.SHAPE_CASTER) > .5f;}, () => {return true;}}
        };

        public static float GetClosestDistance(ShapeCast3D caster)
        {
            int count = caster.GetCollisionCount();
            float shortestDistance = 100f;
            for (int i = 0; i < count; i++)
            {
                Vector3 point = caster.GlobalPosition;
                point.Y = caster.GetCollisionPoint(i).Y;
                float dis = point.DistanceSquaredTo(caster.GlobalPosition);
                shortestDistance = dis > shortestDistance ? shortestDistance : dis;
            }
            return MathF.Sqrt(shortestDistance);
        }

        public static bool TransferToState(Variables.PlayerStandingState toState)
        {
            return StateMatix[(int)(Variables.Instance.CURRENT_STANDING_STATE), (int)toState]();
        }
    }
}

