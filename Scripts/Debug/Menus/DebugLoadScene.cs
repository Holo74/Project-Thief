using Godot;
using System;

namespace Debug.Menus
{
    public class DebugLoadScene : PopupMenu
    {
        System.Collections.Generic.List<string> LevelList { get; set; }
        public override void _Ready()
        {
            LevelList = new System.Collections.Generic.List<string>();
            Directory filePath = new Directory();
            //GD.Print();
            filePath.Open("res://Scenes/Levels/");
            filePath.ListDirBegin();
            string currentName = "";
            do
            {
                currentName = filePath.GetNext();
                if (!filePath.CurrentIsDir())
                {
                    if (!currentName.Empty() && currentName.Contains(".tscn"))
                    {
                        LevelList.Add(currentName);
                        AddItem(currentName);
                    }
                }
            }
            while (!currentName.Empty());

            Connect("id_pressed", this, nameof(LoadLevel));
        }

        private void LoadLevel(int id)
        {
            Management.Game.GameManager.Instance.LoadScene("res://Scenes/Levels/" + LevelList[id]);
        }
    }
}