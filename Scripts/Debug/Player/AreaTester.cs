using Godot;
using System;

public partial class AreaTester : Area3D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += Testing;
        AreaEntered += Testing2;
    }

    public void Testing(Node3D body)
    {
        GD.Print(body.Name + " colliding with " + Name);
    }

    public void Testing2(Node3D body)
    {
        GD.Print(body.Name + " area with " + Name);
    }
}
