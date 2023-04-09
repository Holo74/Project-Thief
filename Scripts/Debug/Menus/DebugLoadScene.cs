using Godot;
using System;

namespace Debug.Menus
{
    public partial class DebugLoadScene : PopupMenu
    {
        [Export(PropertyHint.Dir)]
        private string PathDirectory { get; set; }
        System.Collections.Generic.List<string> LevelList { get; set; }
        public override void _Ready()
        {
            LevelList = new System.Collections.Generic.List<string>();
            DirAccess filePath = DirAccess.Open(PathDirectory);
            string currentName = "";
            //GD.Print();
            filePath.ListDirBegin();
            do
            {
                currentName = filePath.GetNext();
                if (!filePath.CurrentIsDir())
                {
                    if (currentName.Contains(".tscn"))
                    {
                        string name = currentName.Substring(0, currentName.IndexOf('.'));
                        LevelList.Add(name + ".tscn");
                        AddItem(name);
                    }
                }
            }
            while (currentName.Length > 0);

            // Connect("id_pressed", new Callable(this, nameof(LoadLevel)));
            IdPressed += LoadLevel;
        }

        private void LoadLevel(long id)
        {
            Management.Game.GameManager.Instance.LoadScene(PathDirectory + "/" + LevelList[((int)id)], new Action[] { Management.Game.GameManager.Instance.SetupPlayer });
        }
    }
}