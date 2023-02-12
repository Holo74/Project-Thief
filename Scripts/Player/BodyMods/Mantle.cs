using Godot;
using System;

namespace Player.BodyMods
{
    public class Mantle : Spatial
    {
        private Area HeadSpace { get; set; }
        private Area LowerLedgeSpace { get; set; }
        private Area UpperLedgeSpace { get; set; }
        public RayCast UpperLedge { get; private set; }
        public RayCast LowerLedge { get; private set; }
        private int InHeadSpace { get; set; }
        private int InUpperLedgeSpace { get; set; }
        private int InLowerLedgeSpace { get; set; }

        [Signal]
        private delegate void SetHeadNumber(string output);
        [Signal]
        private delegate void SetBodyNumber(string output);
        [Signal]
        private delegate void SetFootNumber(string output);

        public override void _Ready()
        {
            HeadSpace = GetNode<Area>("Spacer");
            UpperLedgeSpace = GetNode<Area>("UpperBodyLedge");
            LowerLedgeSpace = GetNode<Area>("LowerBodyLedge");
            LowerLedge = GetNode<RayCast>("BottomFloor");
            UpperLedge = GetNode<RayCast>("MiddleFloor");
            HeadSpace.Connect("body_entered", this, nameof(EnterHeadSpace));
            HeadSpace.Connect("body_exited", this, nameof(ExitHeadSpace));
            UpperLedgeSpace.Connect("body_entered", this, nameof(EnterUpperLedgeSpace));
            UpperLedgeSpace.Connect("body_exited", this, nameof(ExitUpperLedgeSpace));
            LowerLedgeSpace.Connect("body_entered", this, nameof(EnterLowerLedgeSpace));
            LowerLedgeSpace.Connect("body_exited", this, nameof(ExitLowerLedgeSpace));
            Variables.StandingChangedTo += CrouchChange;
            Variables.OnFloorChange += ChangeMonitors;
        }

        private void ChangeMonitors(bool state)
        {
            if (Variables.CURRENT_STANDING_STATE == Variables.PlayerStandingState.Crouching)
            {
                if (state)
                {
                    if (UpperLedge.Enabled)
                    {
                        LowerLedge.Enabled = true;
                        LowerLedgeSpace.Monitoring = true;
                        UpperLedge.Enabled = false;
                        UpperLedgeSpace.Monitoring = false;
                        InUpperLedgeSpace = 0;
                    }
                }
            }

        }

        private void CrouchChange(Variables.PlayerStandingState current)
        {
            if (current == Variables.PlayerStandingState.Crouching)
            {
                // GD.Print("ON the floor: " + Variables.ON_FLOOR);
                if (Variables.ON_FLOOR)
                {
                    LowerLedge.Enabled = true;
                    LowerLedgeSpace.Monitoring = true;
                    UpperLedge.Enabled = false;
                    UpperLedgeSpace.Monitoring = false;
                    InUpperLedgeSpace = 0;
                }
                else
                {
                    UpperLedge.Enabled = true;
                    UpperLedgeSpace.Monitoring = true;
                    LowerLedge.Enabled = false;
                    LowerLedgeSpace.Monitoring = false;
                    InLowerLedgeSpace = 0;
                }
            }
            else
            {
                //GD.Print("Enabled lower ledge");
                LowerLedge.Enabled = true;
                UpperLedge.Enabled = true;
                UpperLedgeSpace.Monitoring = true;
                LowerLedgeSpace.Monitoring = true;
            }
        }

        public bool CanMoveForwardUpper()
        {
            return InUpperLedgeSpace == 0 && !UpperLedge.IsColliding();
        }

        public bool CanMoveForwardLower()
        {
            return InLowerLedgeSpace == 0 && !LowerLedge.IsColliding();
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            Variables.StandingChangedTo -= CrouchChange;
        }

        public bool CanMantle()
        {
            // GD.Print((InHeadSpace == 0 && (InUpperLedgeSpace > 0 || InLowerLedgeSpace > 0)) + " Can Mantle");
            return InHeadSpace == 0 && (InUpperLedgeSpace > 0 || InLowerLedgeSpace > 0);
        }

        private void EnterHeadSpace(Node body)
        {
            if ((body is PlayerManager))
                return;
            InHeadSpace += 1;
            EmitSignal(nameof(SetHeadNumber), "Head: " + InHeadSpace);
        }

        private void ExitHeadSpace(Node body)
        {
            if ((body is PlayerManager))
                return;
            InHeadSpace -= 1;
            EmitSignal(nameof(SetHeadNumber), "Head: " + InHeadSpace);
        }

        private void EnterUpperLedgeSpace(Node body)
        {
            if ((body is PlayerManager))
                return;
            InUpperLedgeSpace += 1;
            EmitSignal(nameof(SetBodyNumber), "Body: " + InUpperLedgeSpace);
        }

        private void ExitUpperLedgeSpace(Node body)
        {
            if ((body is PlayerManager))
                return;
            InUpperLedgeSpace -= 1;
            EmitSignal(nameof(SetBodyNumber), "Body: " + InUpperLedgeSpace);
        }

        private void EnterLowerLedgeSpace(Node body)
        {
            if ((body is PlayerManager))
                return;
            InLowerLedgeSpace += 1;
            EmitSignal(nameof(SetFootNumber), "Foot: " + InLowerLedgeSpace);
        }

        private void ExitLowerLedgeSpace(Node body)
        {
            if ((body is PlayerManager))
                return;
            InLowerLedgeSpace -= 1;
            EmitSignal(nameof(SetFootNumber), "Foot: " + InLowerLedgeSpace);
        }
    }

}
