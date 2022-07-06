using Godot;
using System;

namespace Interfaces.Interactions
{
    public interface IHealth
    {
        void ReceiveHealthUpdate(Player.Handlers.Health.InteractionTypes type, int amount);
    }

}
