using Godot;
using System;

namespace Environment.Resources
{
    public class CamoInstance : Resource
    {
        [Export]
        public Texture Camo { get; set; }
        [Export(PropertyHint.Range, "0, 100")]
        public int Priority { get; set; }
        [Export]
        public Environment.Resources.SoundDictionary Sound { get; set; }
    }

}
