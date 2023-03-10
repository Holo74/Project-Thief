using Godot;
using System;

namespace Environment.Interactables
{
    public class ClimbableVines : StaticBody, IInteract
    {
        [Export]
        private Vector3 WorldRight { get; set; }
        private bool canInteract = false;

        public override void _Ready()
        {
            GetNode<Area>("Area").Connect("body_entered", this, nameof(PlayerEnteredBody));
            GetNode<Area>("Area").Connect("body_exited", this, nameof(PlayerLeftBody));
        }

        private void PlayerEnteredBody(Node body)
        {
            if (body is Player.PlayerManager)
            {
                canInteract = true;
            }
        }

        private void PlayerLeftBody(Node body)
        {
            if (body is Player.PlayerManager)
            {
                canInteract = false;
                if (Player.Variables.Instance.MOVEMENT is Player.Movement.Climbing)
                {
                    Player.Variables.Instance.RESET_MOVEMENT();
                }
            }
        }

        public void Interact()
        {
            Player.Variables.Instance.MOVEMENT = new Player.Movement.Climbing(WorldRight);
        }

        public bool CanInteract()
        {
            return canInteract;
        }
    }

}
