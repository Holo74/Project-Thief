using Godot;
using System;

namespace Player.GUI
{
    public class HealthBar : Control
    {
        private Label HealthNumber { get; set; }
        private ProgressBar Health { get; set; }
        [Export]
        private NodePath ToPlayer { get; set; }
        public override void _Ready()
        {
            Health = GetNode<ProgressBar>("Health Bar");
            HealthNumber = GetNode<Label>("Health Number");
            GetNode<PlayerManager>(ToPlayer).PlayerHealth.OnHealthChange += UpdateHealth;
        }

        public void UpdateHealth(int value)
        {
            int restricted = (value - 1) % 100;
            Health.Value = restricted;
            HealthNumber.Text = restricted.ToString();
        }
    }

}
