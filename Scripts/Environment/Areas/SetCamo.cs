using Godot;
using System;

namespace Environment.Areas
{
    [Tool]
    public partial class SetCamo : Node
    {
        [Export]
        private Shape3D AreaShape
        {
            get { return areaShape; }
            set { ChangeShape(value); }
        }
        private Shape3D areaShape = null;

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

        private void ChangeShape(Shape3D changeTo)
        {
            if (!Engine.IsEditorHint())
            {
                return;
            }
            areaShape = changeTo;
            GetNode<CollisionShape3D>("Area3D/CollisionShape3D").Shape = changeTo;
        }
    }

}
