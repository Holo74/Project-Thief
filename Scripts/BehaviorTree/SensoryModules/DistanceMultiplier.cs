using Godot;
using System;
using MonoCustomResourceRegistry;

[RegisteredType(nameof(DistanceMultiplier), "", nameof(Resource))]
public partial class DistanceMultiplier : Resource
{
    public DistanceMultiplier(Godot.Collections.Dictionary<string, double> input)
    {
        Multiplier = input["Multiplier"];
        Range = input["Range"];
    }

    public DistanceMultiplier()
    {

    }

    [Export(PropertyHint.Range, "0,10,.1")]
    public double Multiplier { get; set; }
    [Export(PropertyHint.Range, "0,1")]
    public double Range { get; set; }

    public override string ToString()
    {
        return "Range: " + Range.ToString() + "\nMultiplier: " + Multiplier.ToString();
    }
}
