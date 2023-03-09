using Godot;
using System;

namespace Debug.VisibilityIndex
{
    public partial class Display : RichTextLabel
    {
        public override void _Process(double delta)
        {
            string output = "Total value: " + Player.PlayerManager.Instance.GetStealthValue().ToString();
            output += "\nLight value: " + Player.PlayerQuickAccess.LIGHT.CurrentLight;
            output += "\nBase Camo: " + Player.Variables.Instance.CAMO.BaseVisibility;
            Text = output;
        }
    }
}

