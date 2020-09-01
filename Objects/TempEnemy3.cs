using System;
using Godot;

namespace Debugmancer.Objects
{
	public class TempEnemy3 : KinematicBody2D
	{
		private readonly PackedScene _bulletScene = (PackedScene) ResourceLoader.Load("res://Objects/EnemyBullet.tscn");
		private int _shots;
		private bool _canShoot = true;
		private bool _burstStarted;
		private readonly Timer _burstCoolDown = new Timer();
		private readonly Timer _shotCoolDown = new Timer();

		public override void _Ready()
		{
			AddChild(_burstCoolDown);
			AddChild(_shotCoolDown);
			_burstCoolDown.Connect("timeout", this, "_on_Burst_timeout");
			_shotCoolDown.Connect("timeout", this, "_on_Shot_timeout");
		}

		public override void _Process(float delta)
		{
			if (_shots == 4 && !_burstStarted)
			{
				StartBurstTimer();
				_burstStarted = true;
				_canShoot = false;
			}

			if (_canShoot) StartShotTimer();
		}

		private void StartBurstTimer()
		{
			var waitTime = new Random();
			_burstCoolDown.WaitTime = (float) (waitTime.NextDouble() * (2.5 - .95) + .95);
			_burstCoolDown.Start();
		}

		private void StartShotTimer()
		{
			var waitTime = new Random();
			_canShoot = false;
			_shotCoolDown.WaitTime = (float) (waitTime.NextDouble() * (.4 - .1) + .1);
			_shotCoolDown.Start();
		}

		private void _on_Burst_timeout()
		{
			_burstCoolDown.Stop();
			_shots = 0;
			_canShoot = true;
			_burstStarted = false;
		}

		private void _on_Shot_timeout()
		{
			_shotCoolDown.Stop();
			_shots += 1;
			SpawnBullet();
		}


		private void SpawnBullet()
		{
			_canShoot = true;
			var bullet1 = (EnemyBullet) _bulletScene.Instance();
			bullet1.Speed = 100;

			//UP

			bullet1.GlobalPosition = GetNode<Position2D>("BulletSpawns/Up1/Position2D").Position;
			bullet1.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Up1/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Up1/Position2D").AddChild(bullet1);

			var bullet2 = (EnemyBullet) _bulletScene.Instance();
			bullet2.Speed = 100;

			bullet2.GlobalPosition = GetNode<Position2D>("BulletSpawns/Up2/Position2D").Position;
			bullet2.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Up2/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Up2/Position2D").AddChild(bullet2);

			var bullet3 = (EnemyBullet) _bulletScene.Instance();
			bullet3.Speed = 100;

			bullet3.GlobalPosition = GetNode<Position2D>("BulletSpawns/Up3/Position2D").Position;
			bullet3.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Up3/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Up3/Position2D").AddChild(bullet3);

			//DOWN

			var bullet4 = (EnemyBullet) _bulletScene.Instance();
			bullet4.Speed = 100;

			bullet4.GlobalPosition = GetNode<Position2D>("BulletSpawns/Down1/Position2D").Position;
			bullet4.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down1/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Down1/Position2D").AddChild(bullet4);

			var bullet5 = (EnemyBullet) _bulletScene.Instance();
			bullet5.Speed = 100;

			bullet5.GlobalPosition = GetNode<Position2D>("BulletSpawns/Down2/Position2D").Position;
			bullet5.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down2/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Down2/Position2D").AddChild(bullet5);

			var bullet6 = (EnemyBullet) _bulletScene.Instance();
			bullet6.Speed = 100;

			bullet6.GlobalPosition = GetNode<Position2D>("BulletSpawns/Down3/Position2D").Position;
			bullet6.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down3/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Down3/Position2D").AddChild(bullet6);

			//LEFT

			var bullet7 = (EnemyBullet) _bulletScene.Instance();
			bullet7.Speed = 100;

			bullet7.GlobalPosition = GetNode<Position2D>("BulletSpawns/Left1/Position2D").Position;
			bullet7.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left1/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Left1/Position2D").AddChild(bullet7);

			var bullet8 = (EnemyBullet) _bulletScene.Instance();
			bullet8.Speed = 100;

			bullet8.GlobalPosition = GetNode<Position2D>("BulletSpawns/Left2/Position2D").Position;
			bullet8.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left2/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Left2/Position2D").AddChild(bullet8);

			var bullet9 = (EnemyBullet) _bulletScene.Instance();
			bullet9.Speed = 100;

			bullet9.GlobalPosition = GetNode<Position2D>("BulletSpawns/Left3/Position2D").Position;
			bullet9.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left3/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Left3/Position2D").AddChild(bullet9);

			//RIGHT

			var bullet10 = (EnemyBullet) _bulletScene.Instance();
			bullet10.Speed = 100;

			bullet10.GlobalPosition = GetNode<Position2D>("BulletSpawns/Right1/Position2D").Position;
			bullet10.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right1/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Right1/Position2D").AddChild(bullet10);

			var bullet11 = (EnemyBullet) _bulletScene.Instance();
			bullet11.Speed = 100;

			bullet11.GlobalPosition = GetNode<Position2D>("BulletSpawns/Right2/Position2D").Position;
			bullet11.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right2/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Right2/Position2D").AddChild(bullet11);

			var bullet12 = (EnemyBullet) _bulletScene.Instance();
			bullet12.Speed = 100;

			bullet12.GlobalPosition = GetNode<Position2D>("BulletSpawns/Right3/Position2D").Position;
			bullet12.Direction =
				Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right3/Position2D").RotationDegrees);
			GetNode<Position2D>("BulletSpawns/Right3/Position2D").AddChild(bullet12);
		}
	}
}