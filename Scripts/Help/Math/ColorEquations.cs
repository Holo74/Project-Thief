using Godot;
using System;

namespace Help.Math
{
    public static class ColorEquations
    {
        public static float CompareTwoTextures(Texture one, Texture two)
        {
            // Image img1 = one.GetData();
            // Image img2 = two.GetData();

            return CompareTexturesPercentage(ColorAverageInFifths(one), ColorAverageInFifths(two));
        }

        // Holy shit I did my own code very wrong
        [Obsolete]
        private static Color[,] ImageToColorArray(Image input)
        {
            Color[,] outputing = new Color[(int)input.GetSize().x, (int)input.GetSize().y];
            for (int x = 0; x < input.GetSize().x; x++)
            {
                for (int y = 0; y < input.GetSize().y; y++)
                {
                    outputing[x, y] = input.GetPixel(x, y);
                }
            }
            return outputing;
        }

        private static float CompareTexturesPercentage(Color[,] one, Color[,] two, float matching = 0.05f)
        {
            int totalMatches = 0;
            bool[,] matched = new bool[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (ColorMatchInTexture(one[i, j], two, matching, ref matched))
                    {
                        totalMatches++;
                    }
                }
            }
            return totalMatches / 25f;
        }

        private static bool ColorMatchInTexture(Color c, Color[,] patterns, float matching, ref bool[,] matched)
        {
            bool match = false;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (matched[i, j])
                    {
                        continue;
                    }
                    if (ColorMatches(c, patterns[i, j], matching))
                    {
                        matched[i, j] = true;
                        return true;
                    }
                }
            }
            return match;
        }

        private static bool ColorMatches(Color first, Color second, float matching)
        {
            float rMean = (first.r + second.r) / 2.0f;
            float r = (first.r - second.r);
            float g = first.g - second.g;
            float b = first.b - second.b;
            float output = Mathf.Sqrt(((int)((512 + rMean) * r * r) >> 8) + 4 * g * g + ((int)((767 - rMean) * b * b) >> 8));
            return output < matching;
        }

        private static Color[,] ColorAverageInFifths(Texture Camo)
        {
            Color[,] Colors = new Color[5, 5];
            Image source = Camo.GetData();
            if (source.IsCompressed())
            {
                source.Decompress();
            }
            source.Lock();
            int fifthX = source.GetWidth() / 5;
            int fifthY = source.GetHeight() / 5;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    int total = (x * 5 + (y + 1));
                    Colors[x, y] = AverageColor((x * fifthX), (x + 1) * fifthX, y * fifthY, (y + 1) * fifthY, source);
                    //break;
                    // System.Threading.Thread t = new System.Threading.Thread(
                    //     () =>
                    //     {

                    //     });
                    // t.Start();
                }
                //break;
            }
            return Colors;
        }

        private static Color AverageColor(int startX, int endX, int startY, int endY, Image image)
        {
            float r = 0;
            float g = 0;
            float b = 0;
            int stepX = (endX - startX);
            int stepY = (endY - startY);
            int stepFX = stepX / 5;
            int stepFY = stepY / 5;
            float count = stepX * stepY;
            if ((endX - startX) / 5 == 0)
            {
                for (int i = startX; i < endX; i++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        Color c = image.GetPixel(i, y);
                        r += c.r;
                        g += c.g;
                        b += c.b;
                    }
                }
                r /= count;
                g /= count;
                b /= count;
            }
            else
            {
                for (int x = 0; x < 5; x++)
                {
                    for (int y = 0; y < 5; y++)
                    {
                        Color c = AverageColor(startX + (x * stepFX), startX + (x + 1) * stepFX, startY + y * stepFY, startY + (y + 1) * stepFY, image);
                        r += c.r;
                        g += c.g;
                        b += c.b;
                    }
                }
                r /= 25.0f;
                g /= 25.0f;
                b /= 25.0f;
            }

            return new Color(r, g, b);
        }

        public static float ColorIntensity(Color c)
        {
            return (Mathf.Clamp(Mathf.Round((ConvertToLumaUse(c.r, 0.2126f) + ConvertToLumaUse(c.g, 0.7152f) + ConvertToLumaUse(c.b, 0.0722f)) * 10), 0f, 10f) / 10);
        }

        private static float ConvertToLumaUse(float value, float multiplier)
        {
            float holder = value;
            holder = Mathf.Pow(holder, 2.2f);
            return (holder * multiplier);
        }
    }

}
