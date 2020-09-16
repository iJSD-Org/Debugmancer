using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Debugmancer.Objects.Bullets;
using Debugmancer.Objects.Enemies.Spider.States;
using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects.Enemies.Spider
{
	public class Entity : KinematicBody2D
	{
		[Signal]
		public delegate void StateChanged();
		public State CurrentState;
		public Stack<State> StateStack = new Stack<State>();
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();
		private readonly PackedScene _bulletScene = (PackedScene)ResourceLoader.Load("res://Objects/Bullets/EnemyBullet.tscn");
		private KinematicBody2D _player;
		private readonly Random _random = new Random();
		private bool _canShoot;
		private int _shots;

		public override void _Ready()
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Chase");
			StatesMap.Add("Chase", GetNode("States/Chase"));
			StatesMap.Add("Stagger", GetNode("States/Stagger"));
			StatesMap.Add("Idle", GetNode("States/Idle"));
			StatesMap.Add("Wander", GetNode("States/Wander"));

			CurrentState = (Idle)GetNode("States/Idle");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished), this, nameof(ChangeState));
			}

			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));

			GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.15 - .1) + .1);

			StateStack.Push((State)StatesMap["Idle"]);
			ChangeState("Idle");
		}

		public override void _PhysicsProcess(float delta)
		{
			GD.Print(GetNode<VisibilityNotifier2D>("VisibilityNotifier2D").IsOnScreen());
			CurrentState.Update(this, delta);
		}

		private void ShootTimer_timeout()
		{
			GetNode<Timer>("ShootTimer").Stop();
			if (_canShoot)
			{
				EnemyBullet bullet = (EnemyBullet)_bulletScene.Instance();
				GD.Print(Position);
				bullet.Speed = 85;
				bullet.Position = Position;
				bullet.Direction = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Normalized();
				GetParent().AddChild(bullet);
				if (++_shots == 20)
				{
					_shots = 0;
					GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (2.5 - .95) + .95);
					GetNode<Timer>("ShootTimer").Start();
				}
				else
				{
					GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.4 - .1) + .1);
					GetNode<Timer>("ShootTimer").Start();
				}
			}

			else
			{
				GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.15 - .1) + .1);
				GetNode<Timer>("ShootTimer").Start();
			}
		}

		public void Hitbox_BodyEntered(Area2D body)
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
			_player = GetParent().GetNode<KinematicBody2D>("Player");
			GetNode<Timer>("ShootTimer").Start();
			_canShoot = true;
			ChangeState("Chase");
		}
		private void _on_VisibilityNotifier2D_screen_exited()
		{
			_canShoot = false;
			ChangeState("Idle");
		}
		public async void OnHealthChanged(int health)
		{
			Modulate = Color.ColorN("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
			{
				Globals.Score += (int)Math.Ceiling(75 * Globals.ScoreMultiplier);
				GetParent().GetNode<KinematicBody2D>("Player").GetNode<Label>("HUD/Score").Text = $"Score: {Globals.Score}";
				ChangeState("Dead");
			}
		}

		private void ChangeState(string stateName)
		{
			GD.Print(stateName);
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
