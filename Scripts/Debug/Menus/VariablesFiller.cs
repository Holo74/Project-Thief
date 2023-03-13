using Godot;
using System;

namespace Debug.Menus
{
    public partial class VariablesFiller : Node
    {
        Control ChildNode { get; set; }

        private System.Collections.Generic.List<VariablesContainers.SetFloatVariable> Variables { get; set; }

        public override void _Ready()
        {
            Variables = new System.Collections.Generic.List<VariablesContainers.SetFloatVariable>();
            ChildNode = GetNode<Control>("VBoxContainer");
            int marginFiller = 0;
            HBoxContainer row = null;
            foreach (System.Reflection.PropertyInfo prop in typeof(Player.Variables).GetProperties())
            {
                if (prop.PropertyType.IsPrimitive)
                {
                    if (prop.GetValue(Player.Variables.Instance) is double)
                    {
                        marginFiller = AdjustMargin<double>(marginFiller, ref row);
                        VariablesContainers.SetFloatVariable menu = GD.Load<PackedScene>("res://Scenes/Prefabs/MenuItems/FloatVariableSetter.tscn").Instantiate<VariablesContainers.SetFloatVariable>();
                        row.AddChild(menu);
                        Variables.Add(menu);
                        menu.Init((value) => { prop.SetValue(Player.Variables.Instance, value); }, () => { return (double)prop.GetValue(Player.Variables.Instance); }, prop.Name);
                        if (marginFiller != 3)
                        {
                            row.AddChild(new HSeparator());
                        }
                    }

                }
            }
        }

        public void ResetVariables()
        {
            new Player.Variables().INIT();
            foreach (VariablesContainers.SetFloatVariable SFV in Variables)
            {
                SFV.SetValuesFromDatabase();
            }
            Player.PlayerManager.Instance.ConnectVariablesToPlayer();
        }

        private int AdjustMargin<T>(int current, ref HBoxContainer container)
        {
            if (container is null || current == 3)
            {
                MarginContainer margin = new MarginContainer();
                margin.CustomMinimumSize = new Vector2(0, 100);
                ChildNode.AddChild(margin);
                container = new HBoxContainer();
                margin.AddChild(container);
                current = 0;
            }
            return current + 1;
        }
    }

}
