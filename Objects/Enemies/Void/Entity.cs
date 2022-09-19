using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Debugmancer.Objects.Bullets;
using Debugmancer.Objects.Enemies.Void.States;
using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects.Enemies.Void
{
	public partial class Entity : CharacterBody2D
	{
		[Signal]
		public delegate void StateChangedEventHandler();
		public State CurrentState;
		public Stack<State> StateStack = new Stack<State>();
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();
		private readonly PackedScene _bulletScene = (PackedScene)ResourceLoader.Load("res://Objects/Bullets/ShotgunBullet.tscn");
		private CharacterBody2D _player;
		private readonly Random _random = new Random();
		private int _shots;

		public override void _Ready()
		{
			StatesMap.Add("Chase", GetNode("States/Chase"));
			StatesMap.Add("Stagger", GetNode("States/Stagger"));
			StatesMap.Add("Idle", GetNode("States/Idle"));
			StatesMap.Add("Wander", GetNode("States/Wander"));

			CurrentState = (Idle)GetNode("States/Idle");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished),new Callable(this,nameof(ChangeState)));
			}

			GetNode("Health").Connect(nameof(Health.HealthChanged),new Callable(this,nameof(OnHealthChanged)));

			GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.15 - .1) + .1);

			StateStack.Push((State)StatesMap["Idle"]);
			ChangeState("Idle");
		}

		public override void _PhysicsProcess(double delta)
		{
			CurrentState.Update(this, (float)delta);
		}

		private void ShootTimer_timeout()
		{
			GetNode<Timer>("ShootTimer").Stop();
			ShotgunBullet bullet = (ShotgunBullet)_bulletScene.Instantiate();
			bullet.Speed = 140;
			bullet.Position = Position;
			bullet.Rotation = (_player.Position - GlobalPosition).Angle();
			bullet.Direction = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Normalized();
			GetParent().AddChild(bullet);
			if (++_shots == 3)
			{
				_shots = 0;
				GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (1.5 - .95) + .95);
				GetNode<Timer>("ShootTimer").Start();
			}
			else
			{
				GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.7 - .3) + .3);
				GetNode<Timer>("ShootTimer").Start();
			}

		}

		public void Hitbox_BodyEntered(Node2D body)
		{
			Health health = (Health)GetNode("Health");

			if (body.IsInGroup("playerBullet"))
			{
				if (health.CurrentHealth - Globals.PlayerDamage > 0) health.Damage(Globals.PlayerDamage);
				else health.Damage(Globals.PlayerDamage - (health.CurrentHealth - Globals.PlayerDamage));
			}

			if (body.IsInGroup("playerCritBullet"))
			{
				if (health.CurrentHealth - (Globals.PlayerDamage * 2) > 0)
				{
					ChangeState("Stagger");
					health.Damage(Globals.PlayerDamage * 2);
				}
				else health.Damage(Globals.PlayerDamage - (health.CurrentHealth - (Globals.PlayerDamage * 2)));
			}
		}

		private void _on_VisibilityNotifier2D_screen_entered()
		{
			_player = GetParent().GetNode<CharacterBody2D>("Player");
			GetNode<Timer>("ShootTimer").Start();
			ChangeState("Chase");
		}
		private void _on_VisibilityNotifier2D_screen_exited()
		{
			GetNode<Timer>("ShootTimer").Stop();
			GD.Print("VOID EXITED");
			ChangeState("Idle");
		}
		public async void OnHealthChanged(int health)
		{
			Modulate = new Color("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
			{
				Globals.Score += (int)Math.Ceiling(125 * Globals.ScoreMultiplier);
				GetParent().GetNode<CharacterBody2D>("Player").GetNode<Label>("HUD/Score").Text = $"Score: {Globals.Score}";
				ChangeState("Dead");
			}
		}

		private void ChangeState(string stateName)
		{
			CurrentState.Exit(this);
			if (stateName == "Previous")
			{
				StateStack.Pop();
			}
			else if (stateName == "Dead")
			{
				QueueFree();
				return;
			}
			else if (stateName == "Stagger")
			{
				StateStack.Push((State)StatesMap[stateName]);
			}
			else
			{
				StateStack.Pop();
				StateStack.Push((State)StatesMap[stateName]);
			}

			CurrentState = StateStack.Peek();

			// Pass target to Chase State
			if (stateName == "Chase")
			{
				((Chase)CurrentState).Init((Player.Entity)_player);
			}

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}
	}
}
