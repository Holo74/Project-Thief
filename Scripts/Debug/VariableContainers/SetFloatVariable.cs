using Godot;
using System;

namespace Debug.VariablesContainers
{
    public partial class SetFloatVariable : Panel
    {
        // private enum Variables { Gravity, Jump, Walking };
        // [Export]
        // private Variables SelectedVariable { get; set; }
        private HSlider Slider { get; set; }
        private LineEdit Line { get; set; }
        private Label Naming { get; set; }
        public Action<double> SetValue;
        public Func<double> GetValue;
        public override void _Ready()
        {
            Slider = GetChild<HSlider>(0);
            Line = GetChild<LineEdit>(2);
            Naming = GetChild<Label>(1);
            Slider.ValueChanged += SliderChanged;

            // Slider.Connect("value_changed", new Callable(this, nameof(SliderChanged)));
            Line.TextSubmitted += TextChanged;
            // Line.Connect("text_entered", new Callable(this, nameof(TextChanged)));
            // switch (SelectedVariable)
            // {
            //     case Variables.Instance.Gravity:
            //         Slider.Value = Player.Variables.Instance.GRAVITY_STRENGTH;
            //         SliderChanged(Player.Variables.Instance.GRAVITY_STRENGTH);
            //         break;
            //     case Variables.Instance.Jump:
            //         Slider.Value = Player.Variables.Instance.JUMP_STRENGTH;
            //         SliderChanged(Player.Variables.Instance.JUMP_STRENGTH);
            //         break;
            //     case Variables.Instance.Walking:
            //         Slider.Value = Player.Variables.Instance.STANDING_SPEED;
            //         SliderChanged(Player.Variables.Instance.STANDING_SPEED);
            //         break;
            // }
        }

        public void Init(Action<double> setting, Func<double> getter, String name)
        {
            SetValue = setting;
            GetValue = getter;
            Naming.Text = name;
            SetValuesFromDatabase();
        }

        public void SetValuesFromDatabase()
        {
            double temp = GetValue();
            Slider.SetValueNoSignal(temp);
            Line.Text = temp.ToString();
        }

        private void TextChanged(string input)
        {
            double convert = double.Parse(input);
            convert = Mathf.Clamp(convert, 0, 100);
            Slider.Value = convert;
        }

        private void SliderChanged(double value)
        {
            Line.Text = value.ToString();
            UpdateValue(value);
        }

        private void UpdateValue(double value)
        {
            SetValue(value);
        }
    }

}
