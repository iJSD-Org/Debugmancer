using Godot;
using System;

public class Pattern4 : KinematicBody2D
{
    private PackedScene _shotgun_scene = (PackedScene)GD.Load("res://Objects/ShotgunBullet.tscn");
    private KinematicBody2D _player;
    private bool _canShoot = true;

    private Timer _shootTimer = new Timer();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = GetParent().GetNode("Player") as KinematicBody2D;
        _shootTimer.WaitTime = 1.0f;
        _shootTimer.Connect("timeout", this, "on_timer_timeout");
        AddChild(_shootTimer);
    }

    public override void _Process(float delta)
    {
        if(_canShoot)
        {
            SpawnBullet();
        }
    }
    private void on_timer_timeout()
    {
        _shootTimer.Stop();
        _canShoot = true;
    }

    private void SpawnBullet()
    {
    
        var bullet = (ShotgunBullet)_shotgun_scene.Instance();    
        bullet.speed = 200;
        bullet.Position = Position;
        bullet.Rotation = (_player.Position - GlobalPosition).Angle();
        GD.Print(bullet.Rotation);
        bullet.dir = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Normalized();
        GetParent().AddChild(bullet);
        _shootTimer.Start();
        _canShoot = false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
