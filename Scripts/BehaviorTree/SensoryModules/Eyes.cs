using Godot;
using System;
using System.Linq;

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
        private Godot.Collections.Array<DistanceMultiplier> RangesAndMultipliers { get; set; }
        private System.Collections.Generic.List<DistanceMultiplier> RanMult { get; set; }
        [Export]
        private Godot.Collections.Array<string> Testing { get; set; }
        public override void _Ready()
        {
            RanMult = RangesAndMultipliers.OrderBy(x => x.Range).ToList(); ;
            foreach (var item in RanMult)
            {
                GD.Print(item);
            }
            // RangesAndMultipliers.OrderBy(x => x.Range);
            // foreach (DistanceMultiplier item in RangesAndMultipliers)
            // {
            //     GD.Print(item.GetType());
            // }
            // RangesAndMultipliers = new Godot.Collections.Array<DistanceMultiplier>();
            // Godot.Collections.Array a = Distances.Data.AsGodotDictionary()["Distances"].AsGodotArray();
            // foreach (Godot.Collections.Dictionary<string, double> dic in a)
            // {
            //     RangesAndMultipliers.Add(new DistanceMultiplier(dic));
            // }
        }

        public override void _Process(double delta)
        {



        }

        public double GetValues()
        {
            float distanceToPlayer = GlobalPosition.DistanceTo(Player.PlayerManager.Instance.GlobalPosition);
            return SensoryAmount(UpperCast.IsColliding(), distanceToPlayer) + SensoryAmount(MiddleCast.IsColliding(), distanceToPlayer) + SensoryAmount(LowerCast.IsColliding(), distanceToPlayer);
        }

        private double SensoryAmount(bool seen, float distance)
        {
            if (seen)
            {
                return 0.0;
            }
            float length = UpperCast.TargetPosition.Length();
            int i = 0;
            for (; i < RanMult.Count - 1 && RanMult[i].Range * length < distance; i++) ;
            return RanMult[i].Multiplier * VisionSensitivity;
        }
    }
}

