using Godot;
using System;

namespace Player.BodyMods
{
    public partial class Mantle : Node3D
    {
        private Area3D HeadSpace { get; set; }
        private Area3D LowerLedgeSpace { get; set; }
        private Area3D UpperLedgeSpace { get; set; }
        public RayCast3D UpperLedge { get; private set; }
        public RayCast3D LowerLedge { get; private set; }
        private int InHeadSpace { get; set; }
        private int InUpperLedgeSpace { get; set; }
        private int InLowerLedgeSpace { get; set; }

        [Signal]
        public delegate void SetHeadNumberEventHandler(string output);
        [Signal]
        public delegate void SetBodyNumberEventHandler(string output);
        [Signal]
        public delegate void SetFootNumberEventHandler(string output);

        public override void _Ready()
        {
            HeadSpace = GetNode<Area3D>("Spacer");
            UpperLedgeSpace = GetNode<Area3D>("UpperBodyLedge");
            LowerLedgeSpace = GetNode<Area3D>("LowerBodyLedge");
            LowerLedge = GetNode<RayCast3D>("BottomFloor");
            UpperLedge = GetNode<RayCast3D>("MiddleFloor");
            HeadSpace.BodyEntered += EnterHeadSpace;
            HeadSpace.BodyExited += ExitHeadSpace;
            UpperLedgeSpace.BodyEntered += EnterUpperLedgeSpace;
            UpperLedgeSpace.BodyExited += ExitUpperLedgeSpace;
            LowerLedgeSpace.BodyEntered += EnterLowerLedgeSpace;
            LowerLedgeSpace.BodyExited += ExitLowerLedgeSpace;
            LowerLedgeSpace.AreaEntered += EnterLowerLedgeSpace;
            LowerLedgeSpace.AreaExited += ExitLowerLedgeSpace;
            Variables.Instance.StandingChangedTo += CrouchChange;
            Variables.Instance.OnFloorChange += ChangeMonitors;
        }

        private void ChangeMonitors(bool state)
        {
            if (Variables.Instance.CURRENT_STANDING_STATE == Variables.PlayerStandingState.Crouching)
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
                // GD.Print("ON the floor: " + Variables.Instance.ON_FLOOR);
                if (Variables.Instance.ON_FLOOR)
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
            Variables.Instance.StandingChangedTo -= CrouchChange;
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
            GD.Print("upper body space entered by: " + body.Name);
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
            EmitSignal(nameof(SetFootNumber), Variant.CreateFrom("Foot: " + InLowerLedgeSpace));
        }

        private void ExitLowerLedgeSpace(Node body)
        {
            if ((body is PlayerManager))
                return;
            InLowerLedgeSpace -= 1;
            EmitSignal(nameof(SetFootNumber), Variant.CreateFrom("Foot: " + InLowerLedgeSpace));
        }
    }

}
