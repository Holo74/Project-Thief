using Godot;
using System;

namespace Player.Handlers
{
    public class CamoHandler
    {
        private Texture CurrentBodyCamo { get; set; }
        private System.Collections.Generic.List<CamoInstance> SurroundingTexture { get; set; }
        private Texture PrioritySurroundingTexture { get; set; }
        private float CurrentCamoMatch { get; set; }

        private int BodyShowValue { get; set; }

        public int BaseVisibility { get; private set; }
        private int CurrentCamoIndex { get; set; }

        private int[] StandingStateToBodyValueMap =
        {
            50, // Standing
            75, // Crouched
            100 // Crawling
        };

        public void Init()
        {
            Variables.StandingChangedTo += UpdateBodyValue;
            SurroundingTexture = new System.Collections.Generic.List<CamoInstance>();
        }

        private void UpdateBodyValue(Variables.PlayerStandingState state)
        {
            BodyShowValue = StandingStateToBodyValueMap[(int)state];
            BaseVisibility = (BodyShowValue - 80) + Mathf.RoundToInt(CurrentCamoMatch * 16) * 5;
        }

        public void UpdateCurrentBodyCamo(Texture newCamo)
        {
            CurrentBodyCamo = newCamo;
            UpdateCamoMatch();
        }

        public void AddSurroundingTexture(CamoInstance camo)
        {
            SurroundingTexture.Add(camo);
            if (SurroundingTextureChanges())
            {
                // GD.Print("Added texture");
                UpdateCamoMatch();
            }
        }

        public void RemoveSurroundingTexture(CamoInstance camo)
        {
            // GD.Print("Removed texture");
            SurroundingTexture.Remove(camo);
            if (SurroundingTextureChanges())
            {
                UpdateCamoMatch();
            }

        }

        private bool SurroundingTextureChanges()
        {
            int priority = 10000;
            CamoInstance c = new CamoInstance();
            for (int i = 0; i < SurroundingTexture.Count; i++)
            {
                if (SurroundingTexture[i].Priority < priority)
                {
                    c = SurroundingTexture[i];
                    priority = SurroundingTexture[i].Priority;
                    CurrentCamoIndex = i;
                }
            }
            if (PrioritySurroundingTexture != c.Camo)
            {
                PrioritySurroundingTexture = c.Camo;
                return true;
            }
            return false;
        }

        public AudioStream GetSound()
        {
            if (CurrentCamoIndex >= SurroundingTexture.Count || CurrentCamoIndex < 0)
            {
                return null;
            }
            return SurroundingTexture[CurrentCamoIndex].Sound.GetRandomSound();
        }

        public float GetSoundVolume()
        {
            // GD.Print(CurrentCamoIndex + " Current Camo Index\n" + SurroundingTexture.Count + " Count of the surrounding texture");
            if (CurrentCamoIndex >= SurroundingTexture.Count || CurrentCamoIndex < 0)
            {
                return 0f;
            }
            return SurroundingTexture[CurrentCamoIndex].Sound.Loudness;
        }

        public void UpdateCamoMatch()
        {
            if (PrioritySurroundingTexture is null || CurrentBodyCamo is null)
            {
                return;
            }
            // GD.Print("Calculate body shit");
            CurrentCamoMatch = Help.Math.ColorEquations.CompareTwoTextures(CurrentBodyCamo, PrioritySurroundingTexture);
            // GD.Print("Finished calculating");
            BaseVisibility = (BodyShowValue - 80) + Mathf.RoundToInt(CurrentCamoMatch * 16) * 5;
        }

        public struct CamoInstance
        {
            public Texture Camo { get; set; }
            public int Priority { get; set; }
            public Environment.Resources.SoundDictionary Sound { get; set; }
        }
    }
}

