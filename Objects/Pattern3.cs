using Godot;
using System;

public class TempEnemy3 : KinematicBody2D
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
        var bullet1 = (EnemyBullet)_bullet_scene.Instance();    
        bullet1.speed = 100;

        //UP

        bullet1.GlobalPosition = GetNode<Position2D>("BulletSpawns/Up1/Position2D").Position;
        bullet1.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Up1/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Up1/Position2D").AddChild(bullet1);

        var bullet2 = (EnemyBullet)_bullet_scene.Instance();    
        bullet2.speed = 100;

        bullet2.GlobalPosition = GetNode<Position2D>("BulletSpawns/Up2/Position2D").Position;
        bullet2.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Up2/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Up2/Position2D").AddChild(bullet2);

        var bullet3 = (EnemyBullet)_bullet_scene.Instance();    
        bullet3.speed = 100;

        bullet3.GlobalPosition = GetNode<Position2D>("BulletSpawns/Up3/Position2D").Position;
        bullet3.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Up3/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Up3/Position2D").AddChild(bullet3);

        //DOWN

        var bullet4 = (EnemyBullet)_bullet_scene.Instance();    
        bullet4.speed = 100;

        bullet4.GlobalPosition = GetNode<Position2D>("BulletSpawns/Down1/Position2D").Position;
        bullet4.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down1/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Down1/Position2D").AddChild(bullet4);

        var bullet5 = (EnemyBullet)_bullet_scene.Instance();    
        bullet5.speed = 100;

        bullet5.GlobalPosition = GetNode<Position2D>("BulletSpawns/Down2/Position2D").Position;
        bullet5.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down2/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Down2/Position2D").AddChild(bullet5);

        var bullet6 = (EnemyBullet)_bullet_scene.Instance();    
        bullet6.speed = 100;

        bullet6.GlobalPosition = GetNode<Position2D>("BulletSpawns/Down3/Position2D").Position;
        bullet6.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Down3/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Down3/Position2D").AddChild(bullet6);

        //LEFT

        var bullet7 = (EnemyBullet)_bullet_scene.Instance();    
        bullet7.speed = 100;

        bullet7.GlobalPosition = GetNode<Position2D>("BulletSpawns/Left1/Position2D").Position;
        bullet7.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left1/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Left1/Position2D").AddChild(bullet7);

        var bullet8 = (EnemyBullet)_bullet_scene.Instance();    
        bullet8.speed = 100;

        bullet8.GlobalPosition = GetNode<Position2D>("BulletSpawns/Left2/Position2D").Position;
        bullet8.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left2/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Left2/Position2D").AddChild(bullet8);

        var bullet9 = (EnemyBullet)_bullet_scene.Instance();    
        bullet9.speed = 100;

        bullet9.GlobalPosition = GetNode<Position2D>("BulletSpawns/Left3/Position2D").Position;
        bullet9.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Left3/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Left3/Position2D").AddChild(bullet9);

        //RIGHT

        var bullet10 = (EnemyBullet)_bullet_scene.Instance();    
        bullet10.speed = 100;

        bullet10.GlobalPosition = GetNode<Position2D>("BulletSpawns/Right1/Position2D").Position;
        bullet10.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right1/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Right1/Position2D").AddChild(bullet10);

        var bullet11 = (EnemyBullet)_bullet_scene.Instance();    
        bullet11.speed = 100;

        bullet11.GlobalPosition = GetNode<Position2D>("BulletSpawns/Right2/Position2D").Position;
        bullet11.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right2/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Right2/Position2D").AddChild(bullet11);

        var bullet12 = (EnemyBullet)_bullet_scene.Instance();    
        bullet12.speed = 100;

        bullet12.GlobalPosition = GetNode<Position2D>("BulletSpawns/Right3/Position2D").Position;
        bullet12.dir = Vector2.Down.Rotated(GetNode<Position2D>("BulletSpawns/Right3/Position2D").RotationDegrees);
        GetNode<Position2D>("BulletSpawns/Right3/Position2D").AddChild(bullet12);

        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
