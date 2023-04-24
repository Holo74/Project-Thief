using Godot;
using System;

namespace Management.Game
{
	public partial class GameManager : Node
	{
		public static GameManager Instance { get; private set; }
		public delegate void StateChange(bool b);
		public static event StateChange PlayingChange;
		private string LoaderPath { get; set; }
		private bool LoadingScene { get; set; } = false;
		private ProgressBar LoadingBar { get; set; }

		private bool InPausableMoment { get; set; }

		public RandomNumberGenerator Generator { get; set; }
		public string FavoriteGame { get; set; }
		public static void Start()
		{
			PLAYING = true;
			Input.MouseMode = Input.MouseModeEnum.Captured;
			Instance.GetTree().Paused = false;
			Instance.InPausableMoment = true;
		}

		public static void Pause()
		{
			PLAYING = false;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			Instance.GetTree().Paused = true;
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

		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed("Menu") && InPausableMoment)
			{
				if (PLAYING)
				{
					Pause();
				}
				else
				{
					Start();
				}
			}
			if (LoadingScene)
			{
				Godot.Collections.Array a = new Godot.Collections.Array();
				// GD.Print("Loading scene");
				if (ResourceLoader.LoadThreadedGetStatus(LoaderPath, a) == ResourceLoader.ThreadLoadStatus.InProgress)
				{
					LoadingBar.Value = ((float)a[0]);
					return;
				}
				LoadingScene = false;
				PackedScene scene = (PackedScene)ResourceLoader.LoadThreadedGet(LoaderPath);
				if (GetTree().ChangeSceneToPacked(scene) == Error.Ok)
				{
					CallDeferred(nameof(SetupSceneTree));
				}

			}
		}

		public Action DoAfterLoad;
		private void SetupSceneTree()
		{
			DoAfterLoad?.Invoke();
			DoAfterLoad = null;
			GetNode<Control>("LoadingScreen").Visible = false;
		}

		public void SetupPlayer()
		{
			PlayingChange = null;
			new Player.Variables().INIT();
			Player.PlayerManager player = (Player.PlayerManager)ResourceLoader.Load<PackedScene>("res://Scenes/Characters/Player/Player.tscn").Instantiate();
			GetTree().CurrentScene.AddChild(player);
			Godot.Collections.Array<Node> group = GetTree().GetNodesInGroup("Start");
			if (group.Count > 0)
			{
				Transform3D t = ((Node3D)group[0]).Transform;

				player.Transform = t.Translated(Vector3.Up * .5f);
			}
			// Temp assign camo
			Player.Variables.Instance.CAMO.UpdateCurrentBodyCamo(ResourceLoader.Load<Texture2D>("res://Textures/Camo Patterns Test/AnotherCamo.jpg"));
			Start();
		}

		public void QuitToMainMenu()
		{
			LoadScene("res://Scenes/Menus/MainMenu.tscn");
			PLAYING = false;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			Instance.GetTree().Paused = false;
			Instance.InPausableMoment = false;
		}

		// Make a function to better load in the player
		public void LoadScene(string path, Action[] doOnLoad = null)
		{
			doOnLoad ??= new Action[0];
			foreach (Action a in doOnLoad)
			{
				DoAfterLoad += a;
			}
			LoaderPath = path;
			ResourceLoader.LoadThreadedRequest(LoaderPath);
			LoadingScene = true;
			LoadingBar.Value = 0;
			GetNode<Control>("LoadingScreen").Visible = true;
		}

	}
}

