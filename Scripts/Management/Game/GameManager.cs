using Godot;
using System;

namespace Management.Game
{
    public class GameManager : Node
    {
        public static GameManager Instance { get; private set; }
        public delegate void StateChange(bool b);
        public static event StateChange PlayingChange;
        private ResourceInteractiveLoader Loader { get; set; }
        private bool LoadingScene { get; set; } = false;
        private ProgressBar LoadingBar { get; set; }
        public RandomNumberGenerator Generator { get; set; }
        public string FavoriteGame { get; set; }
        public static void Start()
        {
            PLAYING = true;
        }

        public static void Pause()
        {
            PLAYING = false;
        }

        private static bool playing = false;
        public static bool PLAYING
        {
            get { return playing; }
            set { playing = value; PlayingChange?.Invoke(value); }
        }

        public override void _Ready()
        {
            base._Ready();
            FavoriteGame = "Metroid";
            Generator = new RandomNumberGenerator();
            Generator.Seed = FavoriteGame.Hash();
            Instance = this;
            LoadingBar = GetNode<ProgressBar>("LoadingScreen/CenterContainer/VBoxContainer/ProgressBar");
            GetNode<Control>("LoadingScreen").Visible = false;
        }

        public override void _Process(float delta)
        {
            if (LoadingScene)
            {
                while (Loader.Poll() == Error.Ok)
                {
                    LoadingBar.Value = Loader.GetStage() / Loader.GetStageCount() * 100;
                }
                LoadingScene = false;
                PackedScene scene = (PackedScene)Loader.GetResource();
                if (GetTree().ChangeSceneTo(scene) == Error.Ok)
                {
                    CallDeferred(nameof(SetupSceneTree));
                }

            }
        }

        private void SetupSceneTree()
        {
            Player.Variables.INIT();
            Player.PlayerManager player = (Player.PlayerManager)ResourceLoader.Load<PackedScene>("res://Scenes/Characters/Player/Player.tscn").Instance();
            GetTree().Root.AddChild(player);
            Godot.Collections.Array group = GetTree().GetNodesInGroup("Start");
            if (group.Count > 0)
            {
                player.Transform = ((Spatial)group[0]).Transform;
            }
            // Temp assign camo
            Player.Variables.CAMO.UpdateCurrentBodyCamo(ResourceLoader.Load<Texture>("res://Textures/Camo Patterns Test/AnotherCamo.jpg"));
            GetNode<Control>("LoadingScreen").Visible = false;
        }

        // Make a function to better load in the player
        public void LoadScene(string path)
        {
            Loader = ResourceLoader.LoadInteractive(path);
            LoadingScene = true;
            LoadingBar.Value = 0;
            GetNode<Control>("LoadingScreen").Visible = true;
        }

    }
}

