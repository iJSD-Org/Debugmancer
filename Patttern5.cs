using Godot;
using System;

public class Patttern5 : KinematicBody2D
{
    private readonly PackedScene _bulletScene = (PackedScene)ResourceLoader.Load("res://Objects/EnemyBullet2.tscn");
	private int _shots;
	private bool _canShoot = true;
    private bool _burstStarted;
	private readonly Timer _burstCoolDown = new Timer();
	private readonly Timer _shotCoolDown = new Timer();
	private KinematicBody2D _player;
	private int life = 5;
	private int playerDamage = 1;

	public override void _Ready()
	{
        _player = GetParent().GetNode("Player") as KinematicBody2D;
        GetNode<Node2D>("BulletSpawn").Rotation = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Angle();
		AddChild(_burstCoolDown);
		AddChild(_shotCoolDown);
        _shotCoolDown.WaitTime = 0.07f;
		_burstCoolDown.Connect("timeout", this, "_on_Burst_timeout");
		_shotCoolDown.Connect("timeout", this, "_on_Shot_timeout");
	}

    public override void _Process(float delta)
	{
		if (_shots == 18 && !_burstStarted)
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
		_burstCoolDown.WaitTime = (float)(waitTime.NextDouble() * (2.0 - .5) + .5);
		_burstCoolDown.Start();
	}

	private void StartShotTimer()
	{
		_canShoot = false;
		_shotCoolDown.Start();
	}

    private void RotateSpawner()
    {
        if (_shots < 6)
        {
            GetNode<Node2D>("BulletSpawn").Rotate(.5f);
        }
        else
        {
            GetNode<Node2D>("BulletSpawn").Rotate(-.5f);
        }
    }

	private void SpawnBullet()
	{
		_canShoot = true;
		var bullet = (EnemyBullet2)_bulletScene.Instance();
		bullet.Speed = 135;
        bullet.Rotation =  GetNode<Node2D>("BulletSpawn").Rotation ;
		bullet.GlobalPosition = Position;
		bullet.Direction = Vector2.Right.Rotated(GetNode<Position2D>("BulletSpawn/Position2D").RotationDegrees);
		GetParent().AddChild(bullet);
        RotateSpawner();
	}

	private void _on_Burst_timeout()
	{
        GetNode<Node2D>("BulletSpawn").Rotation = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Angle();
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
	public void _on_Hitbox_body_entered(Area2D body)
	{
		if (body.IsInGroup("playerBullet")) life -= playerDamage;
		if (body.IsInGroup("playerCritBullet")) life -= playerDamage * 2;
		if(life < 1) QueueFree();
	}	
}
