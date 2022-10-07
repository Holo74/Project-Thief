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
            // // GD.Print(path);
            // Camo.Texture = ResourceLoader.Load<Texture>(path);


            // //source.Crop(100, 100);
            // ImageTexture texture = new ImageTexture();
            // texture.CreateFromImage(source);
            // Camo.Texture = texture;


            // HasPattern = true;
            // CompareTextures(0.05f);
        }








    }

}
