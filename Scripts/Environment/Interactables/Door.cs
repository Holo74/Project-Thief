using Godot;
using System;

namespace Environment.Interactables
{
    public partial class Door : AnimatableBody3D, IInteract
    {
        [Export]
        private bool Closed { get; set; }

        private double ClosedRotation { get; set; }
        private double OpenRotation { get; set; }
        private Tween CurrentlyPlaying { get; set; }
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (Closed)
            {
                ClosedRotation = RotationDegrees.Y;
                OpenRotation = RotationDegrees.Y + 90;
            }
            else
            {
                ClosedRotation = RotationDegrees.Y - 90;
                OpenRotation = RotationDegrees.Y;
            }

            ClosedRotation *= Math.PI / 180;
            OpenRotation *= Math.PI / 180;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {

        }

        public void Interact()
        {
            if (CurrentlyPlaying is not null)
            {
                CurrentlyPlaying.Kill();
            }
            CurrentlyPlaying = CreateTween();
            CurrentlyPlaying.TweenProperty(this, "rotation:y", Closed ? OpenRotation : ClosedRotation, 1);
            Closed = !Closed;
            CurrentlyPlaying.Play();
        }

        public bool CanInteract()
        {
            return true;
        }
    }

}
