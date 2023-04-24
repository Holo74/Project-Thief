using Godot;
using System;
using Player.BodyMods;

namespace Player
{
    // These are all Nodes or references that shouldn't every be reassigned 
    public static class PlayerQuickAccess
    {
        public static Tween TWEEN { get; private set; }
        public static CollisionShape3D UPPER_BODY { get; private set; }
        public static CollisionShape3D LOWER_BODY { get; private set; }
        public static CollisionShape3D FEET { get; private set; }
        public static Camera3D CAMERA { get; private set; }
        public static Handlers.CameraShaker CAMERA_SHAKE { get; private set; }
        public static Node3D BODY_ROTATION { get; private set; }
        public static CharacterBody3D CHARACTER_BODY { get; private set; }
        public static Basis BODY_DIRECTION { get { return BODY_ROTATION.Transform.Basis; } }
        public static Interfaces.Interactions.IHealth HEALTH { get; private set; }

        public static SideWallDetection WALL_DETECTION { get; private set; }
        public static Interactions.Interaction INTERACTION { get; private set; }
        public static Area3D LOWER_BODY_AREA { get; private set; }
        public static Area3D UPPER_BODY_AREA { get; private set; }
        public static RayCast3D FLOOR_CAST { get; private set; }
        public static BodyMods.Mantle MANTLE { get; private set; }
        public static Handlers.LightHandler LIGHT { get; private set; }
        public static DetectionPoints DETECTION_POINTS { get; set; }
        public static ShapeCast3D SHAPE_CASTER { get; set; }

        public static void DisableLowerBody(bool disabled)
        {
            LOWER_BODY.Disabled = disabled;
            FEET.Disabled = disabled;
        }

        public static Tween CreateCameraTween()
        {
            return CAMERA.CreateTween();
        }

        public static void SyncVariables(PlayerManager p)
        {
            TWEEN = p.CreateTween();
            TWEEN.Pause();
            UPPER_BODY = p.GetNode<CollisionShape3D>("UpperBody");
            LOWER_BODY = p.GetNode<CollisionShape3D>("LowerBody");
            FEET = p.GetNode<CollisionShape3D>("Feet");
            CAMERA_SHAKE = p.GetNode<Handlers.CameraShaker>("BodyNode/CameraShaker");
            CAMERA = CAMERA_SHAKE.GetNode<Camera3D>("Camera3D");
            BODY_ROTATION = p;
            WALL_DETECTION = p.GetNode<SideWallDetection>("Wall Detection");
            CHARACTER_BODY = (CharacterBody3D)p;
            HEALTH = (Interfaces.Interactions.IHealth)p;
            INTERACTION = CAMERA.GetNode<Interactions.Interaction>("Pickup Ray");
            INTERACTION.SetInteraction(Player.Interactions.AbstractInteraction.GET_INTERACTION(Interactions.AbstractInteraction.InteractionTypes.Basic));
            UPPER_BODY_AREA = p.GetNode<Area3D>("Upper Body Detection");
            LOWER_BODY_AREA = p.GetNode<Area3D>("Lower Body Detection");
            FLOOR_CAST = p.GetNode<RayCast3D>("Floor Cast");
            MANTLE = p.GetNode<BodyMods.Mantle>("BodyNode/Mantling");
            LIGHT = p.GetNode<Handlers.LightHandler>("Gem");
            DETECTION_POINTS = p.GetNode<DetectionPoints>("PlayerDetectionPoints");
            SHAPE_CASTER = p.GetNode<ShapeCast3D>("ShapeCast3D");
        }
    }

}
