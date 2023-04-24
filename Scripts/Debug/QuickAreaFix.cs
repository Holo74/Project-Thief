using Godot;
using System;

namespace Debug
{
    public partial class QuickAreaFix : Area3D
    {
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            CollisionLayer -= 1;
            CollisionLayer += 1;
        }
    }

}
