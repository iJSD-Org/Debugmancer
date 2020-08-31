using Godot;
using System;

public class Gun : Sprite
{
    [Export] public PackedScene Bullet = ResourceLoader.Load("res://Objects/Bullet.tscn") as PackedScene;
    private bool _canShoot = true;
	[Export] public float BulletSpeed = 1000f;
	[Export] public int BulletCount = 50;
	[Export] public float FireRate = 0.2f;

    public override void _Ready()
    {
        
    }
    public override void _Process(float delta)
    {
        
    }
    public override void _PhysicsProcess(float delta)
    {
        LookAt(GetGlobalMousePosition());
        if (Input.IsActionPressed("click") && _canShoot && BulletCount > 0)
		{
			Shoot();
			GetNode<Label>("HUD/BulletCount").Text = $"Number of Bullets Left: {BulletCount}";
		}
    }
    private void Shoot()
    {
        RigidBody2D bulletInstance = (RigidBody2D)Bullet.Instance();
		bulletInstance.Position = GetNode<Node2D>("GunPoint").GlobalPosition;
		bulletInstance.RotationDegrees = RotationDegrees;
		bulletInstance.ApplyImpulse(new Vector2(0, 0), new Vector2(BulletSpeed, 0).Rotated(Rotation));
		GetTree().Root.AddChild(bulletInstance);
		BulletCount--;
        ShootTimer();
    }
    public async void ShootTimer()
	{
		_canShoot = false;
		await ToSignal(GetTree().CreateTimer(FireRate), "timeout");
		_canShoot = true;
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
