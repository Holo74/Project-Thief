using Godot;
using System;

namespace Environment.Areas
{
    public abstract partial class PlayerArea : Area3D
    {
        public override void _Ready()
        {
            BodyEntered += NodeEntered;
            BodyExited += NodeLeft;
            // Connect("body_entered", new Callable(this, nameof(NodeEntered)));
            // Connect("body_exited", new Callable(this, nameof(NodeLeft)));
        }

        private void NodeEntered(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                PlayerEntered();
            }
        }

        private void NodeLeft(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                PlayerLeft();
            }
        }

        protected abstract void PlayerEntered();
        protected abstract void PlayerLeft();
    }

}
