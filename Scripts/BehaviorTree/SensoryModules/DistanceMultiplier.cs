using Godot;
using System;

public partial class DistanceMultiplier
{
    public DistanceMultiplier(Godot.Collections.Dictionary<string, double> input)
    {
        Multiplier = input["Multiplier"];
        Range = input["Range"];
    }

    public double Multiplier { get; set; }
    public double Range { get; set; }
}
