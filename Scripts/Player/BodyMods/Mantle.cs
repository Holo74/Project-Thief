using Godot;
using System;

namespace Player.BodyMods
{
    public class Mantle : Spatial
    {
        private Area HeadSpace { get; set; }
        private Area LedgeSpace { get; set; }
        private int InHeadSpace { get; set; }
        private int InLedgeSpace { get; set; }
        public override void _Ready()
        {
            HeadSpace = GetNode<Area>("Spacer");
            LedgeSpace = GetNode<Area>("LedgeDetection");
            HeadSpace.Connect("body_entered", this, nameof(EnterHeadSpace));
            HeadSpace.Connect("body_exited", this, nameof(ExitHeadSpace));
            LedgeSpace.Connect("body_entered", this, nameof(EnterLedgeSpace));
            LedgeSpace.Connect("body_exited", this, nameof(ExitLedgeSpace));
        }

        public bool CanMantle()
        {
            return InHeadSpace == 0 && InLedgeSpace > 0;
        }

        private void EnterHeadSpace(Node body)
        {
            InHeadSpace += 1;
        }

        private void ExitHeadSpace(Node body)
        {
            InHeadSpace -= 1;
        }

        private void EnterLedgeSpace(Node body)
        {
            InLedgeSpace += 1;
        }

        private void ExitLedgeSpace(Node body)
        {
            InLedgeSpace -= 1;
        }
    }

}
