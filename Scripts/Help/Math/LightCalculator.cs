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
            // tex1.Resize(1, 1, Image.Interpolation.Lanczos);
            // Color c1 = tex1.GetPixel(0, 0);
            // c1.A = 1;
            // float bright1 = c1.SrgbToLinear().Luminance;
            // tex2.Resize(1, 1, Image.Interpolation.Lanczos);
            // Color c2 = tex2.GetPixel(0, 0);
            // c2.A = 1;
            // float bright2 = c2.SrgbToLinear().Luminance;
            float bright1 = LightLevelFromTexture(tex1);
            float bright2 = LightLevelFromTexture(tex2);
            return bright1 > bright2 ? bright1 : bright2;
        }
    }
}
