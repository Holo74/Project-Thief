using Godot;
using System;

namespace Debug.VariablesContainers
{
    public class SetFloatVariable : Panel
    {
        // private enum Variables { Gravity, Jump, Walking };
        // [Export]
        // private Variables SelectedVariable { get; set; }
        private HSlider Slider { get; set; }
        private LineEdit Line { get; set; }
        private Label Naming { get; set; }
        public Action<float> SetValue;
        public Func<float> GetValue;
        public override void _Ready()
        {
            Slider = GetChild<HSlider>(0);
            Line = GetChild<LineEdit>(2);
            Naming = GetChild<Label>(1);
            Slider.Connect("value_changed", this, nameof(SliderChanged));
            Line.Connect("text_entered", this, nameof(TextChanged));
            // switch (SelectedVariable)
            // {
            //     case Variables.Gravity:
            //         Slider.Value = Player.Variables.GRAVITY_STRENGTH;
            //         SliderChanged(Player.Variables.GRAVITY_STRENGTH);
            //         break;
            //     case Variables.Jump:
            //         Slider.Value = Player.Variables.JUMP_STRENGTH;
            //         SliderChanged(Player.Variables.JUMP_STRENGTH);
            //         break;
            //     case Variables.Walking:
            //         Slider.Value = Player.Variables.STANDING_SPEED;
            //         SliderChanged(Player.Variables.STANDING_SPEED);
            //         break;
            // }
        }

        public void Init(Action<float> setting, Func<float> getter, String name)
        {
            SetValue = setting;
            GetValue = getter;
            Naming.Text = name;
            float temp = GetValue();
            Slider.Value = temp;
            SliderChanged(temp);
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
            SetValue(value);
        }
    }

}
