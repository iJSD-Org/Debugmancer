using Godot;
using System;

public class TempEnemy2 : KinematicBody2D
{
    
    private PackedScene _bullet_scene = (PackedScene)ResourceLoader.Load("res://Objects/EnemyBullet.tscn");
    private int _shots = 0;
    private bool _canShoot = true;
    private bool _burstStarted = false;
    private Timer _burstCoolDown = new Timer();
    private Timer _shotCoolDown = new Timer();
    private KinematicBody2D _player;
    public override void _Ready()
    {
        _player = GetParent().GetNode("Player") as KinematicBody2D;
        AddChild(_burstCoolDown);
        AddChild(_shotCoolDown);
        _burstCoolDown.Connect("timeout", this, "_on_Burst_timeout");
        _shotCoolDown.Connect("timeout", this, "_on_Shot_timeout");
    }

    public override void _Process(float delta)
    {
    
        if (_shots == 20 && !_burstStarted)
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
        _burstCoolDown.WaitTime = (float)(waitTime.NextDouble() * (2.5- .95) + .95);
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
        var bullet = (EnemyBullet)_bullet_scene.Instance();
        bullet.speed = 130;    
        bullet.Position = Position;
        bullet.dir = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Normalized();
        GetParent().AddChild(bullet);
    }


}
