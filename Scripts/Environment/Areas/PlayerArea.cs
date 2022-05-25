using Godot;
using System;

namespace Environment.Areas
{
    public abstract class PlayerArea : Area
    {
        public override void _Ready()
        {
            Connect("body_entered", this, nameof(NodeEntered));
            Connect("body_exited", this, nameof(NodeLeft));
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
