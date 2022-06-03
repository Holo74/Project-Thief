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
            Variables.CrouchChange += CrouchChange;
        }

        private void CrouchChange(bool current)
        {
            if (current)
            {
                if (Variables.ON_FLOOR)
                {
                    UpperLedge.Enabled = false;
                    UpperLedgeSpace.Monitoring = false;
                    InUpperLedgeSpace = 0;
                }
                else
                {

                    LowerLedge.Enabled = false;
                    LowerLedgeSpace.Monitoring = false;
                    InLowerLedgeSpace = 0;
                }
            }
            else
            {
                GD.Print("Enabled lower ledge");
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
            Variables.CrouchChange -= CrouchChange;
        }

        public bool CanMantle()
        {
            return InHeadSpace == 0 && (InUpperLedgeSpace > 0 || InLowerLedgeSpace > 0);
        }

        private void EnterHeadSpace(Node body)
        {
            InHeadSpace += 1;
        }

        private void ExitHeadSpace(Node body)
        {
            InHeadSpace -= 1;
        }

        private void EnterUpperLedgeSpace(Node body)
        {
            InUpperLedgeSpace += 1;
            GD.Print(InUpperLedgeSpace + " Added one to upper");
        }

        private void ExitUpperLedgeSpace(Node body)
        {
            InUpperLedgeSpace -= 1;
            GD.Print(InUpperLedgeSpace + " Subtract one to upper");
        }

        private void EnterLowerLedgeSpace(Node body)
        {
            InLowerLedgeSpace += 1;
            GD.Print(body.Name);
            GD.Print(InLowerLedgeSpace + " Added one to lower");
        }

        private void ExitLowerLedgeSpace(Node body)
        {
            InLowerLedgeSpace -= 1;
            GD.Print(InLowerLedgeSpace + " Subtract one to lower");
        }
    }

}
