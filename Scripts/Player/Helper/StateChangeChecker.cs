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
            {() => {return PlayerQuickAccess.UPPER_BODY_AREA.GetOverlappingBodies().Count == 0;}, () => {return true;}, () => {return true;}},
            {() => {return PlayerQuickAccess.UPPER_BODY_AREA.GetOverlappingBodies().Count == 0 && PlayerQuickAccess.LOWER_BODY_AREA.GetOverlappingBodies().Count == 0;}, () => { return PlayerQuickAccess.LOWER_BODY_AREA.GetOverlappingBodies().Count == 0;}, () => {return true;}}
        };
        public static bool TransferToState(Variables.PlayerStandingState toState)
        {
            return StateMatix[(int)(Variables.Instance.CURRENT_STANDING_STATE), (int)toState]();
        }
    }
}

