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

        [Export(PropertyHint.ResourceType)]
        Environment.Resources.CamoInstance Camo { get; set; }

        public override void _Ready()
        {
            base._Ready();
        }

        private void PlayerEntered(Node body)
        {

            // GD.Print("Node enter body");
            if (body is Player.PlayerManager p)
            {
                Player.Variables.Instance.CAMO.AddSurroundingTexture(Camo);
            }
        }

        private void PlayerLeft(Node body)
        {
            if (body is Player.PlayerManager p)
            {
                Player.Variables.Instance.CAMO.RemoveSurroundingTexture(Camo);
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
