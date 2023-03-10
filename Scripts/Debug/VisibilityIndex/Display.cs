using Godot;
using System;

namespace Debug.VisibilityIndex
{
    public class Display : RichTextLabel
    {
        public override void _Process(float delta)
        {
            string output = "Total value: " + Player.PlayerManager.Instance.GetStealthValue().ToString();
            output += "\nLight value: " + Player.PlayerQuickAccess.LIGHT.CurrentLight;
            output += "\nBase Camo: " + Player.Variables.Instance.CAMO.BaseVisibility;
            Text = output;
        }
    }
}

