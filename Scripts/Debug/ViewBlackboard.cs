using Godot;
using System;

namespace Debug
{
    public partial class ViewBlackboard : Label3D
    {
        [Export]
        private BehaviorTree.BehaviorController Controller { get; set; }
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            string outputting = "";
            foreach (var item in Controller.BlackBoard)
            {
                outputting += item.Key.ToString() + " : " + item.Value.ToString() + "\n";
            }
            Text = outputting;
        }
    }

}
