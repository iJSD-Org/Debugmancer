using System;
using System.Collections.Generic;
using Godot;

namespace Debugmancer.Objects.TempEnemy1
{
	public class TempEnemy1 : KinematicBody2D
	{
		[Export] public int Speed;
		[Export] public int StoppingDistance;
		[Export] public int RetreatDistance;


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

			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));

			GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.7 - .1) + .1);
			GetNode<Timer>("ShootTimer").Start();
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
			if (++_shots == 3)
			{
				_shots = 0;
				GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (2.0 - .5) + .5);
				GetNode<Timer>("ShootTimer").Start();
			}
			else
			{
				GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.7 - .1) + .1);
				GetNode<Timer>("ShootTimer").Start();
			}
		}

		public void Hitbox_BodyEntered(Area2D body)
		{
			Health health = (Health)GetNode("Health");
			if (body.IsInGroup("playerBullet")) health.Damage(1);

			if (body.IsInGroup("playerCritBullet")) health.Damage(2);
		}

		public void OnHealthChanged(int health)
		{
			if (health == 0)
				QueueFree();
		}
	}
}
