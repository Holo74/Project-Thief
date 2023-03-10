using Godot;
using System;

namespace Debug.Menus
{
    public class VariablesFiller : Node
    {
        Control ChildNode { get; set; }

        public override void _Ready()
        {
            ChildNode = GetNode<Control>("VBoxContainer");
            int marginFiller = 0;
            HBoxContainer row = null;
            foreach (System.Reflection.PropertyInfo prop in typeof(Player.Variables).GetProperties())
            {
                if (prop.PropertyType.IsPrimitive)
                {
                    if (prop.GetValue(Player.Variables.Instance) is float)
                    {
                        marginFiller = AdjustMargin<float>(marginFiller, ref row);
                        VariablesContainers.SetFloatVariable menu = GD.Load<PackedScene>("res://Scenes/Prefabs/MenuItems/FloatVariableSetter.tscn").Instance<VariablesContainers.SetFloatVariable>();
                        row.AddChild(menu);
                        menu.Init((value) => { prop.SetValue(Player.Variables.Instance, value); }, () => { return (float)prop.GetValue(Player.Variables.Instance); }, prop.Name);
                        if (marginFiller != 3)
                        {
                            row.AddChild(new HSeparator());
                        }
                    }

                }
            }
        }

        private int AdjustMargin<T>(int current, ref HBoxContainer container)
        {
            if (container is null || current == 3)
            {
                MarginContainer margin = new MarginContainer();
                margin.RectMinSize = new Vector2(0, 100);
                ChildNode.AddChild(margin);
                container = new HBoxContainer();
                margin.AddChild(container);
                current = 0;
            }
            return current + 1;
        }
    }

}
