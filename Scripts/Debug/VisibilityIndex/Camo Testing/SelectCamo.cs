using Godot;
using System;

namespace Debug.VisibilityIndex.CamoTesting
{
    public partial class SelectCamo : FileDialog
    {
        [Export]
        private NodePath[] CamoSelectors { get; set; }
        public override void _Ready()
        {

        }
        private void TextureSelect(int selector)
        {
            // This code doesn't work and I'm 90 percent sure that it isn't being used.
            // Godot.Collections.Array<Godot.Collections.Dictionary> list = GetSignalConnectionList("file_selected");
            // foreach (Godot.Collections.Dictionary x in list)
            // {
            //     Disconnect(x["signal"].ToString(), new Callable((Godot.Object)(x["target"]), x["method"].ToString()));
            // }
            // Popup_();
            // FileSelected += () => {};
            // Connect("file_selected", new Callable(GetNode(CamoSelectors[selector]), nameof(SetCamo.SetCamoTexture)), null, (uint)(ConnectFlags.OneShot));
        }
    }

}
