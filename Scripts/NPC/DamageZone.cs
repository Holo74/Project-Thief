using Godot;
using System;

public partial class DamageZone : Area3D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += Damage;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Damage(Node body)
    {
        GD.Print(body.Name);
        if (body is Player.PlayerManager p)
        {
            p.ReceiveHealthUpdate(Player.Handlers.Health.InteractionTypes.Falling, -10);
        }
    }
}
