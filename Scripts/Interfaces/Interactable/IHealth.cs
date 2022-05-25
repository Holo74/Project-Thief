using Godot;
using System;

namespace Interfaces.Interactions
{
    public interface IHealth
    {
        void Damage(int damage);
        void Heal(int healing);
    }

}
