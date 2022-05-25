using Godot;
using System;
using Player.BodyMods;

namespace Player
{
    public static class PlayerQuickAccess
    {
        public static Tween TWEEN { get; private set; }
        public static CollisionShape UPPER_BODY { get; private set; }
        public static CollisionShape LOWER_BODY { get; private set; }
        public static Camera CAMERA { get; private set; }
        public static Spatial BODY_ROTATION { get; private set; }
        public static KinematicBody KINEMATIC_BODY { get; private set; }
        public static Basis BODY_DIRECTION { get { return BODY_ROTATION.Transform.basis; } }
        public static Interfaces.Interactions.IHealth HEALTH { get; private set; }

        public static SideWallDetection WALL_DETECTION { get; private set; }
        public static Interactions.Interaction INTERACTION { get; private set; }
        public static Area UPPER_BODY_AREA { get; private set; }
        public static RayCast FLOOR_CAST { get; private set; }
        public static void SyncVariables(PlayerManager p)
        {
            TWEEN = p.GetNode<Tween>("Tween");
            UPPER_BODY = p.GetNode<CollisionShape>("UpperBody");
            LOWER_BODY = p.GetNode<CollisionShape>("LowerBody");
            CAMERA = p.GetNode<Camera>("BodyNode/Camera");
            BODY_ROTATION = p;
            WALL_DETECTION = p.GetNode<SideWallDetection>("Wall Detection");
            KINEMATIC_BODY = (KinematicBody)p;
            HEALTH = (Interfaces.Interactions.IHealth)p;
            INTERACTION = p.GetNode<Interactions.Interaction>("BodyNode/Camera/Pickup Ray");
            INTERACTION.SetInteraction(Player.Interactions.AbstractInteraction.GET_INTERACTION(Interactions.AbstractInteraction.InteractionTypes.Basic));
            UPPER_BODY_AREA = p.GetNode<Area>("Upper Body Detection");
            FLOOR_CAST = p.GetNode<RayCast>("Floor Cast");
        }
    }

}
