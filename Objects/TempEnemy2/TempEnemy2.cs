using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Debugmancer.Objects.TempEnemy1.States;
using Debugmancer.Objects.TempEnemy2.States;
using Godot;

namespace Debugmancer.Objects.TempEnemy2
{
	public class TempEnemy2 : KinematicBody2D
	{
		[Signal]
		public delegate void StateChanged();

		public State CurrentState;
		public Stack<State> StateStack = new Stack<State>();
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();


		private readonly PackedScene _bulletScene = (PackedScene)ResourceLoader.Load("res://Objects/Bullets/EnemyBullet.tscn");
		private KinematicBody2D _player;
		private readonly Random _random = new Random();
		private int _shots;

		public override void _Ready()
		{
			_player = GetParent().GetNode<KinematicBody2D>("Player");

			StatesMap.Add("Chase", GetNode("States/Chase"));
			StatesMap.Add("Stagger", GetNode("States/Stagger"));

			CurrentState = (State)GetNode("States/Chase");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished), this, nameof(ChangeState));
			}

			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));

			GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.7 - .1) + .1);
			GetNode<Timer>("ShootTimer").Start();

			StateStack.Push((State)StatesMap["Chase"]);
			ChangeState("Chase");
		}

		public override void _PhysicsProcess(float delta)
		{
			CurrentState.Update(this, delta);
		}

		private void ShootTimer_timeout()
		{
			GetNode<Timer>("ShootTimer").Stop();
			// Shoot
			EnemyBullet bullet = (EnemyBullet)_bulletScene.Instance();
			bullet.Speed = 135;
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

		public void Hitbox_BodyEntered(Area2D body)
		{
			Health health = (Health)GetNode("Health");

			if (body.IsInGroup("playerBullet")) health.Damage(1);

			if (body.IsInGroup("playerCritBullet") && health.CurrentHealth <= 0)
			{
				health.Damage(2);
				ChangeState("Stagger");
			}
		}

		public async void OnHealthChanged(int health)
		{
			Modulate = Color.ColorN("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
				ChangeState("Dead");
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
				((Chase2)CurrentState).Init((Player.Player)_player);
			}

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}
	}
}
