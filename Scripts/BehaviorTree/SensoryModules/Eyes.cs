using Godot;
using System;

namespace BehaviorTree.SensoryModules
{
    public partial class Eyes : Node3D
    {
        [Export]
        private RayCast3D UpperCast { get; set; }
        [Export]
        private RayCast3D MiddleCast { get; set; }
        [Export]
        private RayCast3D LowerCast { get; set; }
        [Export]
        private double VisionSensitivity { get; set; }

        [Export]
        private Godot.Json Distances { get; set; }
        private System.Collections.Generic.List<DistanceMultiplier> RangesAndMultipliers { get; set; }
        public override void _Ready()
        {
            RangesAndMultipliers = new System.Collections.Generic.List<DistanceMultiplier>();
            Godot.Collections.Array a = Distances.Data.AsGodotDictionary()["Distances"].AsGodotArray();
            foreach (Godot.Collections.Dictionary<string, double> dic in a)
            {
                RangesAndMultipliers.Add(new DistanceMultiplier(dic));
            }
        }

        public override void _Process(double delta)
        {
            float distanceToPlayer = GlobalPosition.DistanceTo(Player.PlayerManager.Instance.GlobalPosition);
            double addToSensory = SensoryAmount(UpperCast.IsColliding(), distanceToPlayer) + SensoryAmount(MiddleCast.IsColliding(), distanceToPlayer) + SensoryAmount(LowerCast.IsColliding(), distanceToPlayer);
        }

        private double SensoryAmount(bool seen, float distance)
        {
            if (seen)
            {
                return 0.0;
            }
            float length = UpperCast.TargetPosition.Length();
            int i = 0;
            for (; i < RangesAndMultipliers.Count - 1 && RangesAndMultipliers[i].Range * length < distance; i++) ;
            return RangesAndMultipliers[i].Multiplier * VisionSensitivity;
        }
    }
}

