using Godot;
using System;

namespace Environment.Areas
{
    [Tool]
    public class SetCamo : Node
    {
        [Export]
        private Shape AreaShape
        {
            get { return areaShape; }
            set { ChangeShape(value); }
        }
        private Shape areaShape = null;
        [Export]
        private Texture Surroundings { get; set; }
        [Export(PropertyHint.Range, "0, 100")]
        private int Priority { get; set; }
        [Export]
        private Resources.SoundDictionary Sound { get; set; }

        private void PlayerEntered(Node body)
        {

            // GD.Print("Node enter body");
            if (body is Player.PlayerManager p)
            {
                // GD.Print("Entered body");
                Player.Handlers.CamoHandler.CamoInstance camo = new Player.Handlers.CamoHandler.CamoInstance();
                camo.Priority = Priority;
                camo.Camo = Surroundings;
                camo.Sound = Sound;
                Player.Variables.CAMO.AddSurroundingTexture(camo);
            }
        }

        private void PlayerLeft(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                // GD.Print("Left body");
                Player.Handlers.CamoHandler.CamoInstance camo = new Player.Handlers.CamoHandler.CamoInstance();
                camo.Priority = Priority;
                camo.Camo = Surroundings;
                camo.Sound = Sound;
                Player.Variables.CAMO.RemoveSurroundingTexture(camo);
            }
        }

        private void ChangeShape(Shape changeTo)
        {
            if (!Engine.EditorHint)
            {
                return;
            }
            areaShape = changeTo;
            GetNode<CollisionShape>("Area/CollisionShape").Shape = changeTo;
        }
    }

}
