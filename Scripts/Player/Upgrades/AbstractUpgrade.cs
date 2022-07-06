using Godot;
using System;

namespace Player.Upgrades
{
    // Used for any upgrades to the player movement or otherwise
    public abstract class AbstractUpgrade : Resource
    {
        public abstract void Applied();
        public abstract void Update(float delta);
        public abstract void Removed();
    }

}
