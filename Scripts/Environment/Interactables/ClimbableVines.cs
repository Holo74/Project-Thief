using Godot;
using System;

namespace Environment.Interactables
{
    public partial class ClimbableVines : StaticBody3D, IInteract
    {
        [Export]
        private Vector3 WorldRight { get; set; }
        private bool canInteract = false;

        public override void _Ready()
        {
            GetNode<Area3D>("Area3D").Connect("body_entered",new Callable(this,nameof(PlayerEnteredBody)));
            GetNode<Area3D>("Area3D").Connect("body_exited",new Callable(this,nameof(PlayerLeftBody)));
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
