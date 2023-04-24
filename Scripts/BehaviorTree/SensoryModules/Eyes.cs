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
        [Export(PropertyHint.Range, ("0,100,.1"))]
        private double DecreasingVisibilityVal { get; set; }

        [Export]
        private BehaviorController BC { get; set; }

        [Export]
        private Godot.Collections.Array<DistanceMultiplier> RangesAndMultipliers { get; set; }
        private System.Collections.Generic.List<DistanceMultiplier> RanMult { get; set; }
        [Export]
        private Godot.Collections.Array<string> Testing { get; set; }
        public override void _Ready()
        {

            RanMult = RangesAndMultipliers.OrderBy(x => x.Range).ToList();
            if (!BC.BlackBoard.ContainsKey(Enums.KeyList.InDirectEye) || !BC.BlackBoard.ContainsKey(Enums.KeyList.DirectEye))
            {
                BC.BlackBoard[Enums.KeyList.InDirectEye] = false;
                BC.BlackBoard[Enums.KeyList.DirectEye] = false;
            }
            if (!BC.BlackBoard.ContainsKey(Enums.KeyList.Sensor))
            {
                BC.BlackBoard.Add(Enums.KeyList.Sensor, 0.0);
            }
            // foreach (var item in RanMult)
            // {
            //     GD.Print(item);
            // }
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
            if (BC.BlackBoard[Enums.KeyList.InDirectEye].AsBool() || BC.BlackBoard[Enums.KeyList.DirectEye].AsBool())
            {
                double currentVal = BC.BlackBoard[Enums.KeyList.Sensor].AsDouble();
                UpperCast.LookAt(Player.PlayerQuickAccess.DETECTION_POINTS.Head.GlobalPosition);
                MiddleCast.LookAt(Player.PlayerQuickAccess.DETECTION_POINTS.Body.GlobalPosition);
                LowerCast.LookAt(Player.PlayerQuickAccess.DETECTION_POINTS.Feet.GlobalPosition);
                double sensorVal = GetValues();
                BC.BlackBoard[Enums.KeyList.SeePlayer] = (sensorVal > .5);
                sensorVal = (sensorVal > .5) ? sensorVal : -DecreasingVisibilityVal;
                BC.BlackBoard[Enums.KeyList.Sensor] = Mathf.Clamp(sensorVal * delta + currentVal, 0, 100);
                BC.BlackBoard[Enums.KeyList.DisturbanceLocation] = Player.PlayerQuickAccess.CHARACTER_BODY.GlobalPosition;
            }
            else
            {
                double currentVal = BC.BlackBoard[Enums.KeyList.Sensor].AsDouble();
                BC.BlackBoard[Enums.KeyList.Sensor] = Mathf.Clamp(-DecreasingVisibilityVal * delta + currentVal, 0, 100);
            }

        }

        public double GetValues()
        {
            float distanceToPlayer = GlobalPosition.DistanceTo(Player.PlayerManager.Instance.GlobalPosition);
            return SensoryAmount(SeesPlayer(UpperCast), distanceToPlayer) + SensoryAmount(SeesPlayer(MiddleCast), distanceToPlayer) + SensoryAmount(SeesPlayer(LowerCast), distanceToPlayer);
        }

        private bool SeesPlayer(RayCast3D caster)
        {
            return caster.IsColliding() && caster.GetCollider() is Player.BodyMods.DetectionPoints;
        }

        private double SensoryAmount(bool seen, float distance)
        {
            if (!seen)
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

