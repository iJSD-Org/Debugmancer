using System;
using System.Threading.Tasks;
using Debugmancer.Objects.Bullets;
using Debugmancer.Objects.Player;

using Godot;

namespace Debugmancer.Objects.Enemies.Computer
{
	public class Entity : KinematicBody2D
	{
		private readonly PackedScene _bulletScene = (PackedScene)ResourceLoader.Load("res://Objects/Bullets/EnemyBullet2.tscn");
		private readonly Random _random = new Random();
		private int _shots;
		private bool _canShoot;
		private int _bulletSpeed = 90;
		public override void _Ready()
		{
			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));
		}

		private void ShootTimer_timeout()
		{
			GetNode<Timer>("ShootTimer").Stop();

			if(_canShoot) SpawnBullet();

			if (++_shots == 6)
			{
				_shots = 0;
				GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (6 - .95) + .95);
				GetNode<Timer>("ShootTimer").Start();
			}
			else
			{
				GetNode<Timer>("ShootTimer").WaitTime = (float)(_random.NextDouble() * (.5 - .1) + .1);
				GetNode<Timer>("ShootTimer").Start();
			}
		}

		private void _on_VisibilityNotifier2D_screen_entered()
		{
			GetNode<Timer>("ShootTimer").Start();
			_canShoot = true;
		}

		private void _on_VisibilityNotifier2D_screen_exited()
		{
			GetNode<Timer>("ShootTimer").Stop();
			_canShoot = false;
		}

		public void Hitbox_BodyEntered(Area2D area)
		{
			Health health = (Health)GetNode("Health");

			if (area.IsInGroup("playerBullet")) 
			{
				if(health.CurrentHealth - Globals.PlayerDamage > 0) health.Damage(Globals.PlayerDamage);
				else health.Damage(Globals.PlayerDamage - (health.CurrentHealth - Globals.PlayerDamage));
			}

			if (area.IsInGroup("playerCritBullet")) 
			{
				if(health.CurrentHealth - (Globals.PlayerDamage * 2) > 0) health.Damage(Globals.PlayerDamage * 2);
				else health.Damage(Globals.PlayerDamage - (health.CurrentHealth - (Globals.PlayerDamage * 2)));
			}
		}

		private void SpawnBullet()
		{
			var bullet1 = (EnemyBullet2)_bulletScene.Instance();
			bullet1.Speed = _bulletSpeed;

			//UP
			bullet1.Position = Position;
			bullet1.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet1.Direction = Vector2.Up.Rotated(GetNode<Position2D>("BulletSpawns/Up1/Position2D").RotationDegrees);
			GetParent().AddChild(bullet1);

			var bullet2 = (EnemyBullet2)_bulletScene.Instance();
			bullet2.Speed = _bulletSpeed;

			bullet2.Position = Position;
			bullet2.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet2.Direction = Vector2.Up.Rotated(GetNode<Position2D>("BulletSpawns/Up2/Position2D").RotationDegrees);
			GetParent().AddChild(bullet2);

			var bullet3 = (EnemyBullet2)_bulletScene.Instance();
			bullet3.Speed = _bulletSpeed;

			bullet3.Position = Position;
			bullet3.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet3.Direction = Vector2.Up.Rotated(GetNode<Position2D>("BulletSpawns/Up3/Position2D").RotationDegrees);
			GetParent().AddChild(bullet3);

			//DOWN

			var bullet4 = (EnemyBullet2)_bulletScene.Instance();
			bullet4.Speed = _bulletSpeed;

			bullet4.GlobalPosition = Position;
			bullet4.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet4.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down1/Position2D").RotationDegrees);
			GetParent().AddChild(bullet4);

			var bullet5 = (EnemyBullet2)_bulletScene.Instance();
			bullet5.Speed = _bulletSpeed;

			bullet5.GlobalPosition = Position;
			bullet5.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet5.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down2/Position2D").RotationDegrees);
			GetParent().AddChild(bullet5);

			var bullet6 = (EnemyBullet2)_bulletScene.Instance();
			bullet6.Speed = _bulletSpeed;

			bullet6.GlobalPosition = Position;
			bullet6.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet6.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down3/Position2D").RotationDegrees);
			GetParent().AddChild(bullet6);

			//LEFT

			var bullet7 = (EnemyBullet2)_bulletScene.Instance();
			bullet7.Speed = _bulletSpeed;

			bullet7.GlobalPosition = Position;
			bullet7.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet7.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left1/Position2D").RotationDegrees);
			GetParent().AddChild(bullet7);

			var bullet8 = (EnemyBullet2)_bulletScene.Instance();
			bullet8.Speed = _bulletSpeed;

			bullet8.GlobalPosition = Position;
			bullet8.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet8.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left2/Position2D").RotationDegrees);
			GetParent().AddChild(bullet8);

			var bullet9 = (EnemyBullet2)_bulletScene.Instance();
			bullet9.Speed = _bulletSpeed;

			bullet9.GlobalPosition = Position;
			bullet9.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet9.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left3/Position2D").RotationDegrees);
			GetParent().AddChild(bullet9);

			//RIGHT

			var bullet10 = (EnemyBullet2)_bulletScene.Instance();
			bullet10.Speed = _bulletSpeed;

			bullet10.GlobalPosition = Position;
			bullet10.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet10.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right1/Position2D").RotationDegrees);
			GetParent().AddChild(bullet10);

			var bullet11 = (EnemyBullet2)_bulletScene.Instance();
			bullet11.Speed = _bulletSpeed;

			bullet11.GlobalPosition = Position;
			bullet11.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet11.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right2/Position2D").RotationDegrees);
			GetParent().AddChild(bullet11);

			var bullet12 = (EnemyBullet2)_bulletScene.Instance();
			bullet12.Speed = _bulletSpeed;

			bullet12.GlobalPosition = Position;
			bullet12.Rotation = (float)(Math.PI / 180) * _random.Next(0, 360);
			bullet12.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right3/Position2D").RotationDegrees);
			GetParent().AddChild(bullet12);
		}

		public async void OnHealthChanged(int health)
		{

			Modulate = Color.ColorN("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
			{
				Globals.Score += (int)Math.Ceiling(100 * Globals.ScoreMultiplier);
				GetParent().GetNode<KinematicBody2D>("Player").GetNode<Label>("HUD/Score").Text = $"Score: {Globals.Score}";
				QueueFree();
			}
		}
	}
}
