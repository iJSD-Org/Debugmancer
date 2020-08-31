using Godot;
using System;

public class TempEnemy3 : KinematicBody2D
{
	private readonly PackedScene _bulletScene = (PackedScene)ResourceLoader.Load("res://Objects/EnemyBullet2.tscn");
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

		if (_canShoot)
		{
			StartShotTimer();
		}
	}

	private void StartBurstTimer()
	{
		Random waitTime = new Random();
		_burstCoolDown.WaitTime = (float)(waitTime.NextDouble() * (2.5 - .95) + .95);
		_burstCoolDown.Start();
	}

	private void StartShotTimer()
	{
		Random waitTime = new Random();
		_canShoot = false;
		_shotCoolDown.WaitTime = (float)(waitTime.NextDouble() * (.4 - .1) + .1);
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
		var bullet1 = (EnemyBullet2)_bulletScene.Instance();
		bullet1.Speed = 100;

		//UP
		bullet1.Position = Position;
		bullet1.Rotation = GetNode<Node2D>("BulletSpawns/Up1").Rotation;
		bullet1.Direction = Vector2.Up.Rotated(GetNode<Position2D>("BulletSpawns/Up1/Position2D").RotationDegrees);
		GetParent().AddChild(bullet1);

		var bullet2 = (EnemyBullet2)_bulletScene.Instance();
		bullet2.Speed = 100;

		bullet2.Position = Position;
		bullet2.Rotation = GetNode<Node2D>("BulletSpawns/Up2").Rotation;
		bullet2.Direction = Vector2.Up.Rotated(GetNode<Position2D>("BulletSpawns/Up2/Position2D").RotationDegrees);
		GetParent().AddChild(bullet2);

		var bullet3 = (EnemyBullet2)_bulletScene.Instance();
		bullet3.Speed = 100;

		bullet3.Position = Position;
		bullet3.Rotation = GetNode<Node2D>("BulletSpawns/Up3").Rotation;
		bullet3.Direction = Vector2.Up.Rotated(GetNode<Position2D>("BulletSpawns/Up3/Position2D").RotationDegrees);
		GetParent().AddChild(bullet3);

		//DOWN

		var bullet4 = (EnemyBullet2)_bulletScene.Instance();
		bullet4.Speed = 100;

		bullet4.GlobalPosition = Position;
		bullet4.Rotation = GetNode<Node2D>("BulletSpawns/Down1").Rotation;
		bullet4.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down1/Position2D").RotationDegrees);
		GetParent().AddChild(bullet4);

		var bullet5 = (EnemyBullet2)_bulletScene.Instance();
		bullet5.Speed = 100;

		bullet5.GlobalPosition = Position;
		bullet5.Rotation = GetNode<Node2D>("BulletSpawns/Down2").Rotation;
		bullet5.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down2/Position2D").RotationDegrees);
		GetParent().AddChild(bullet5);

		var bullet6 = (EnemyBullet2)_bulletScene.Instance();
		bullet6.Speed = 100;

		bullet6.GlobalPosition = Position;
		bullet6.Rotation = GetNode<Node2D>("BulletSpawns/Down3").Rotation;
		bullet6.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down3/Position2D").RotationDegrees);
		GetParent().AddChild(bullet6);

		//LEFT

		var bullet7 = (EnemyBullet2)_bulletScene.Instance();
		bullet7.Speed = 100;

		bullet7.GlobalPosition = Position;
		bullet7.Rotation = GetNode<Node2D>("BulletSpawns/Left1").Rotation;
		bullet7.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left1/Position2D").RotationDegrees);
		GetParent().AddChild(bullet7);

		var bullet8 = (EnemyBullet2)_bulletScene.Instance();
		bullet8.Speed = 100;

		bullet8.GlobalPosition = Position;
		bullet8.Rotation = GetNode<Node2D>("BulletSpawns/Left2").Rotation;
		bullet8.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left2/Position2D").RotationDegrees);
		GetParent().AddChild(bullet8);

		var bullet9 = (EnemyBullet2)_bulletScene.Instance();
		bullet9.Speed = 100;

		bullet9.GlobalPosition = Position;
		bullet9.Rotation = GetNode<Node2D>("BulletSpawns/Left3").Rotation;
		bullet9.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left3/Position2D").RotationDegrees);
		GetParent().AddChild(bullet9);
		
		//RIGHT

		var bullet10 = (EnemyBullet2)_bulletScene.Instance();
		bullet10.Speed = 100;

		bullet10.GlobalPosition = Position;
		bullet10.Rotation = GetNode<Node2D>("BulletSpawns/Right1").Rotation;
		bullet10.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right1/Position2D").RotationDegrees);
		GetParent().AddChild(bullet10);

		var bullet11 = (EnemyBullet2)_bulletScene.Instance();
		bullet11.Speed = 100;

		bullet11.GlobalPosition = Position;
		bullet11.Rotation = GetNode<Node2D>("BulletSpawns/Right2").Rotation;
		bullet11.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right2/Position2D").RotationDegrees);
		GetParent().AddChild(bullet11);

		var bullet12 = (EnemyBullet2)_bulletScene.Instance();
		bullet12.Speed = 100;

		bullet12.GlobalPosition = Position;
		bullet12.Rotation = GetNode<Node2D>("BulletSpawns/Right3").Rotation;
		bullet12.Direction = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right3/Position2D").RotationDegrees);
		GetParent().AddChild(bullet12);
	}
}
