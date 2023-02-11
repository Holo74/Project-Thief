using Godot;
using System;
using Player.BodyMods;

namespace Player
{
    // These are all Nodes or references that shouldn't every be reassigned 
    public static class PlayerQuickAccess
    {
        public static SceneTreeTween TWEEN { get; private set; }
        public static CollisionShape UPPER_BODY { get; private set; }
        public static CollisionShape LOWER_BODY { get; private set; }
        public static CollisionShape FEET { get; private set; }
        public static Camera CAMERA { get; private set; }
        public static Handlers.CameraShaker CAMERA_SHAKE { get; private set; }
        public static Spatial BODY_ROTATION { get; private set; }
        public static KinematicBody KINEMATIC_BODY { get; private set; }
        public static Basis BODY_DIRECTION { get { return BODY_ROTATION.Transform.basis; } }
        public static Interfaces.Interactions.IHealth HEALTH { get; private set; }

        public static SideWallDetection WALL_DETECTION { get; private set; }
        public static Interactions.Interaction INTERACTION { get; private set; }
        public static Area LOWER_BODY_AREA { get; private set; }
        public static Area UPPER_BODY_AREA { get; private set; }
        public static RayCast FLOOR_CAST { get; private set; }
        public static BodyMods.Mantle MANTLE { get; private set; }
        public static Handlers.LightHandler LIGHT { get; private set; }

        public static void DisableLowerBody(bool disabled)
        {
            LOWER_BODY.Disabled = disabled;
            FEET.Disabled = disabled;
        }

        public static SceneTreeTween CreateCameraTween()
        {
            return CAMERA.CreateTween();
        }

        public static void SyncVariables(PlayerManager p)
        {
            TWEEN = p.CreateTween();
            TWEEN.Pause();
            UPPER_BODY = p.GetNode<CollisionShape>("UpperBody");
            LOWER_BODY = p.GetNode<CollisionShape>("LowerBody");
            FEET = p.GetNode<CollisionShape>("Feet");
            CAMERA_SHAKE = p.GetNode<Handlers.CameraShaker>("BodyNode/CameraShaker");
            CAMERA = CAMERA_SHAKE.GetNode<Camera>("Camera");
            BODY_ROTATION = p;
            WALL_DETECTION = p.GetNode<SideWallDetection>("Wall Detection");
            KINEMATIC_BODY = (KinematicBody)p;
            HEALTH = (Interfaces.Interactions.IHealth)p;
            INTERACTION = CAMERA.GetNode<Interactions.Interaction>("Pickup Ray");
            INTERACTION.SetInteraction(Player.Interactions.AbstractInteraction.GET_INTERACTION(Interactions.AbstractInteraction.InteractionTypes.Basic));
            UPPER_BODY_AREA = p.GetNode<Area>("Upper Body Detection");
            LOWER_BODY_AREA = p.GetNode<Area>("Lower Body Detection");
            FLOOR_CAST = p.GetNode<RayCast>("Floor Cast");
            MANTLE = p.GetNode<BodyMods.Mantle>("BodyNode/Mantling");
            LIGHT = p.GetNode<Handlers.LightHandler>("Gem");
        }
    }

}
