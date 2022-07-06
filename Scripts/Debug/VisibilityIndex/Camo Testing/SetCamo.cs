using Godot;
using System;
using System.Threading;

namespace Debug.VisibilityIndex.CamoTesting
{
    public class SetCamo : Node
    {
        private Color[,] Colors;
        private TextureRect Camo;
        [Export]
        private NodePath SetCamoPath { get; set; }
        [Export]
        private NodePath ResultsText { get; set; }
        private SetCamo PartnerPattern { get; set; }
        private bool HasPattern { get; set; } = false;

        public override void _Ready()
        {
            Camo = GetNode<TextureRect>("TextureRect");
            Colors = new Color[5, 5];
            PartnerPattern = GetNode<SetCamo>(SetCamoPath);
        }

        public void SetCamoTexture(string path)
        {
            GD.Print(path);
            Camo.Texture = ResourceLoader.Load<Texture>(path);
            Image source = Camo.Texture.GetData();
            if (source.IsCompressed())
            {
                source.Decompress();
            }
            //source.Crop(100, 100);
            ImageTexture texture = new ImageTexture();
            texture.CreateFromImage(source);
            Camo.Texture = texture;
            source.Lock();
            int fifthX = source.GetWidth() / 5;
            int fifthY = source.GetHeight() / 5;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    int total = (x * 5 + (y + 1));
                    if (total > 25)
                        GD.Print(total + " :Total\n" + x + " :X\n" + y + " :Y");
                    ColorRect rect = GetNode<ColorRect>("GridContainer/ColorRect" + total);
                    rect.Color = AverageColor((x * fifthX), (x + 1) * fifthX, y * fifthY, (y + 1) * fifthY, source);
                    Colors[x, y] = rect.Color;
                    rect.HintTooltip = (rect.Color.ToString());
                    //break;
                    // System.Threading.Thread t = new System.Threading.Thread(
                    //     () =>
                    //     {

                    //     });
                    // t.Start();
                }
                //break;
            }
            HasPattern = true;
            CompareTextures(0.05f);
        }

        private void CompareTextures(float matching)
        {
            GD.Print(matching + " Current floor");
            if (!PartnerPattern.HasPattern)
                return;
            int totalMatches = 0;
            bool[,] matched = new bool[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (ColorMatchInTexture(Colors[i, j], matching, ref matched))
                    {
                        totalMatches++;
                    }
                }
            }
            GetNode<RichTextLabel>(ResultsText).Text = totalMatches + " out of 25 or a " + (totalMatches / 0.25f) + "%";
        }

        private bool ColorMatchInTexture(Color c, float matching, ref bool[,] matched)
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
                    if (ColorMatches(c, PartnerPattern.Colors[i, j], matching))
                    {
                        matched[i, j] = true;
                        return true;
                    }
                }
            }
            return match;
        }

        private bool ColorMatches(Color first, Color second, float matching)
        {
            float rMean = (first.r + second.r) / 2.0f;
            float r = (first.r - second.r);
            float g = first.g - second.g;
            float b = first.b - second.b;
            float output = Mathf.Sqrt(((int)((512 + rMean) * r * r) >> 8) + 4 * g * g + ((int)((767 - rMean) * b * b) >> 8));
            return output < matching;
        }

        private Color AverageColor(int startX, int endX, int startY, int endY, Image image)
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
    }

}
