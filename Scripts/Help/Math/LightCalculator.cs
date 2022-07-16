using Godot;
using System;

namespace Help.Math
{
    public static class LightCalculator
    {
        public static float LightLevelFromTexture(Image texture)
        {
            float currentBrightest = 0f;
            for (int i = 0; i < texture.GetWidth(); i++)
            {
                for (int j = 0; j < texture.GetHeight(); j++)
                {
                    float number = ColorEquations.ColorIntensity(texture.GetPixel(i, j));
                    currentBrightest = (number > currentBrightest) ? number : currentBrightest;
                }
            }
            return currentBrightest;
        }

        public static float GetBrightnessFromTextures(Image tex1, Image tex2)
        {
            float bright1 = LightLevelFromTexture(tex1);
            float bright2 = LightLevelFromTexture(tex2);
            return bright1 > bright2 ? bright1 : bright2;
        }
    }
}
