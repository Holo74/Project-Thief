using Godot;
using System;

namespace BehaviorTree
{
	public partial class BehaviorController : CharacterBody3D
	{
		private Nodes.Base Root { get; set; }
		public Godot.Collections.Dictionary<BehaviorTree.Enums.KeyList, Variant> BlackBoard { get; private set; }
		public NavigationAgent3D NavAgent { get; private set; }
		public AnimationTree AnimTree { get; private set; }
		public Vector3 SetVelocity { get; set; }
		[Export]
		public AudioStreamPlayer3D MouthSoundPlayer { get; set; }
		[Export]
		public AudioStreamPlayer3D FeetSoundPlayer { get; set; }

		private int VelocitySyncCounter { get; set; }

		[Export]
		private float TurnSpeed { get; set; }

		// If the dot product between the safe velocity and the current facing direction are beyond this then turn
		[Export(PropertyHint.Range, ("0,180,1"))]
		private float StartBeyondXDegrees { get; set; }
		public override void _Ready()
		{
			BlackBoard = new Godot.Collections.Dictionary<BehaviorTree.Enums.KeyList, Variant>();
			NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
			AnimTree = GetNode<AnimationTree>("AnimationTree");
			StartBeyondXDegrees = (90 - StartBeyondXDegrees) / 90;
			VelocitySyncCounter = 0;
			BlackBoard.Add(Enums.KeyList.Debugging, "");
		}

		public override void _Process(double delta)
		{
			if (!(Root is null))
			{
				Nodes.Results result = Root.Tick(delta, this);
				if (result == Nodes.Results.Failure)
				{
					//GD.Print("Ended in Failure");
				}
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			if (!SetVelocity.IsEqualApprox(Vector3.Zero))
			{
				NavAgent.SetVelocity(SetVelocity);
				// GD.Print(SetVelocity);
				VelocitySyncCounter++;
				if (VelocitySyncCounter >= 60)
				{
					SetVelocity = Vector3.Zero;
				}
			}
		}

		public void AssignRoot(Nodes.Base root)
		{
			Root = root;
		}

		public void InArea(Node body, bool entered, int name)
		{
			if (body is Player.PlayerManager)
				BlackBoard[(BehaviorTree.Enums.KeyList)name] = entered;
		}

		public void SetVelocityToPhysics(Vector3 vel)
		{
			SetVelocity = vel;
			VelocitySyncCounter = 0;
		}

		public void SafeVelocityComputed(Vector3 velocity)
		{
			// Turn to
			// GD.Print("Turning and at degree: " + velocity.Dot(-GlobalTransform.Basis.Z));
			Vector3 sameLevel = velocity;
			sameLevel.Y = 0;
			GlobalTransform = GlobalTransform.InterpolateWith(GlobalTransform.LookingAt(sameLevel + GlobalPosition, Vector3.Up), ((float)GetPhysicsProcessDeltaTime()) * TurnSpeed);
			Velocity = velocity;
			MoveAndSlide();
		}
	}

}
