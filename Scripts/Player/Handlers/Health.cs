using Godot;
using System;

namespace Player.Handlers
{
    public partial class Health : Resource
    {

        public enum InteractionTypes
        {
            Falling,
            Drowning
        }

        [Export]
        public Resistances[] ResistanceList;

        public delegate void HealthChanged(int value);
        public HealthChanged OnHealthChange;

        public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; private set; } = 100;
        public void ModifyHealth(InteractionTypes type, int mod)
        {
            foreach (Resistances types in ResistanceList)
            {
                if (types.resistant == type)
                {
                    mod += types.amount;
                }
            }
            //GD.Print(mod + " Damage taken");
            CurrentHealth = Mathf.Clamp(CurrentHealth + mod, -1, MaxHealth);
            OnHealthChange(CurrentHealth);
        }
    }
}

