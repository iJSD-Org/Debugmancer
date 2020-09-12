using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Timer = System.Timers.Timer;

namespace Debugmancer.Objects.Player
{
	public class Entity : KinematicBody2D
	{
		[Signal]
		public delegate void StateChanged();

		public PackedScene ScentScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/Scent.tscn");

		public List<Scent> ScentTrail = new List<Scent>();

		public State CurrentState;
		public Stack<State> StateStack = new Stack<State>();
		private readonly Timer _dashCooldownTimer = new Timer();
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();
		public static int score = 0;
		public override void _Ready()
		{
			GetNode<TextureProgress>("HUD/VBoxContainer/Health").MaxValue = 25;
			GetNode<TextureProgress>("HUD/VBoxContainer/Health").Value = 25;
			StatesMap.Add("Idle", GetNode("States/Idle"));
			StatesMap.Add("Move", GetNode("States/Move"));
			StatesMap.Add("Dash", GetNode("States/Dash"));

			CurrentState = (State)GetNode("States/Idle");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished), this, nameof(ChangeState));
			}

			_dashCooldownTimer.AutoReset = false;
			_dashCooldownTimer.Enabled = false;
			_dashCooldownTimer.Interval = 500;

			StateStack.Push((State)StatesMap["Idle"]);
			ChangeState("Idle");
		}

		public override void _Process(float delta)
		{
			Sprite weapon = GetNode<Sprite>("Gun");
			if (Math.Abs(weapon.Rotation) < 90 * (Math.PI / 180)) TurnRight();
			else if (Math.Abs(weapon.Rotation) >= 90 * (Math.PI / 180)) TurnLeft();
		}

		public override void _PhysicsProcess(float delta)
		{
			CurrentState.Update(this, delta);
		}

		public override void _Input(InputEvent @event)
		{
			// Firing is the weapon"s responsibility so the weapon should handle it
			if (@event.IsActionPressed("click"))
			{
				((Gun)GetNode<Sprite>("Gun")).Fire();
				return;
			}
			CurrentState.HandleInput(this, @event);
		}

		private void TurnLeft()
		{
			Sprite weapon = GetNode<Sprite>("Gun");
			weapon.Position = new Vector2(-Mathf.Abs(weapon.Position.x), weapon.Position.y);
			weapon.FlipV = true;
			Sprite player = GetNode<Sprite>("Sprite");
			player.FlipH = true;
		}
		private void TurnRight()
		{
			Sprite weapon = GetNode<Sprite>("Gun");
			weapon.Position = new Vector2(Mathf.Abs(weapon.Position.x), weapon.Position.y);
			weapon.FlipV = false;
			Sprite player = GetNode<Sprite>("Sprite");
			player.FlipH = false;
		}

		#region Signal Receivers
		private void ChangeState(string stateName)
		{
			CurrentState.Exit(this);
			if (stateName == "Previous")
			{
				StateStack.Pop();
			}
			else if (stateName == "Dash")
			{
				if (!_dashCooldownTimer.Enabled)
				{
					_dashCooldownTimer.Start();
					StateStack.Push((State)StatesMap[stateName]);
				}
			}
			else if (stateName == "Dead")
			{
				QueueFree();
				return;
			}
			else
			{
				StateStack.Pop();
				StateStack.Push((State)StatesMap[stateName]);
			}

			CurrentState = StateStack.Peek();

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		public async void OnHealthChanged(int health)
		{
			((Camera)GetNode<Camera2D>("Camera")).StartShake();
			GetNode<TextureProgress>("HUD/VBoxContainer/Health").Value = health;
			GD.Print(GetNode<TextureProgress>("HUD/VBoxContainer/Health").Value);
			Modulate = Color.ColorN("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
				ChangeState("Dead");
		}

		public void AddScent()
		{
			Scent scent = (Scent)ScentScene.Instance();
			scent.Position = Position;
			GetTree().Root.AddChild(scent);

			scent.Init(this);
			ScentTrail.Add(scent);
		}

		public void Hitbox_BodyEntered(Area2D body)
		{
			if (body.IsInGroup("enemy"))
				((Health)GetNode("Health")).Damage(2);
		}

		public void BodyTimer_timeout()
		{
			List<KinematicBody2D> bodies =
				GetNode<Area2D>("Hitbox").GetOverlappingBodies().OfType<KinematicBody2D>().ToList();
			if (bodies.Any(b => b.IsInGroup("enemy")))
			{
				((Health)GetNode("Health")).Damage(2);
			}
		}

		public void Hitbox_AreaEntered(Area2D area)
		{
			if (area.IsInGroup("shotgunBullet"))
				((Health)GetNode("Health")).Damage(5);
			if (area.IsInGroup("enemyBullet"))
				((Health)GetNode("Health")).Damage(1);
		}
		#endregion
	}
}
