using Godot;
using System;

namespace Debug.VariablesContainers
{
    public class SetFloatVariable : Panel
    {
        private enum Variables { Gravity, Jump, Walking };
        [Export]
        private Variables SelectedVariable { get; set; }
        private HSlider Slider { get; set; }
        private LineEdit Line { get; set; }
        public override void _Ready()
        {
            Slider = GetChild<HSlider>(0);
            Line = GetChild<LineEdit>(2);
            Slider.Connect("value_changed", this, nameof(SliderChanged));
            Line.Connect("text_changed", this, nameof(TextChanged));
            switch (SelectedVariable)
            {
                case Variables.Gravity:
                    Slider.Value = Player.Variables.GRAVITY_STRENGTH;
                    SliderChanged(Player.Variables.GRAVITY_STRENGTH);
                    break;
                case Variables.Jump:
                    Slider.Value = Player.Variables.JUMP_STRENGTH;
                    SliderChanged(Player.Variables.JUMP_STRENGTH);
                    break;
                case Variables.Walking:
                    Slider.Value = Player.Variables.SPEED;
                    SliderChanged(Player.Variables.SPEED);
                    break;
            }
        }

        private void TextChanged(string input)
        {
            float convert = float.Parse(input);
            Slider.Value = convert;
        }

        private void SliderChanged(float value)
        {
            Line.Text = value.ToString();
            UpdateValue(value);
        }

        private void UpdateValue(float value)
        {
            switch (SelectedVariable)
            {
                case Variables.Gravity:
                    Player.Variables.GRAVITY_STRENGTH = value;
                    break;
                case Variables.Jump:
                    Player.Variables.JUMP_STRENGTH = value;
                    break;
                case Variables.Walking:
                    Player.Variables.SPEED = value;
                    break;
            }
        }
    }

}
