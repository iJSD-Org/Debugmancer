using Godot;
using System;
using System.Collections;

public class KinematicBody2D : Godot.KinematicBody2D
{
	private Vector2 inputVector = Vector2.Zero;
	private int SPEED = 150;
	private bool isWalking = false;
	private bool canDash = true;
	private PackedScene bullet = ResourceLoader.Load("res://Objects/Bullet.tscn") as PackedScene;
	private float bulletSpeed = 1000f;
	private bool canShoot = true;
	private float fireRate = 0.2f;
	private int bulletCount = 50;
	private void _process(float delta)
	{
		inputVector = GetInput();
		Move();
	}
	private Vector2 GetInput()
	{
		Vector2 velocity = new Vector2();
		LookAt(GetGlobalMousePosition());
		velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		if(Input.IsActionJustPressed("ui_select") && canDash) Dash();
		if (velocity.x != 0 || velocity.y != 0) isWalking = true;
		if(Input.IsActionPressed("click") && canShoot && bulletCount > 0)
		{
			var bulletInstance = bullet.Instance() as RigidBody2D;
			bulletInstance.Position = GetNode<Node2D>("Gun").GlobalPosition;
			bulletInstance.RotationDegrees = RotationDegrees;
			bulletInstance.ApplyImpulse(new Vector2(0,0), new Vector2(bulletSpeed, 0).Rotated(Rotation));
			GetTree().Root.AddChild(bulletInstance);
			bulletCount--;
			UpdateBulletCount(bulletCount);
			ShootTimer();
		}
		return velocity * SPEED;
	}
	private void Move()
	{
		inputVector = MoveAndSlide(inputVector);
		isWalking = false;
	}
	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
	async public void Dash()
	{
		SPEED = 300;
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		SPEED = 150;
		DashTimer();
	}
	async public void DashTimer()
	{
		canDash = false;
		await ToSignal(GetTree().CreateTimer(3), "timeout");
		canDash = true;
	}
	async public void ShootTimer()
	{
		canShoot = false;
		await ToSignal(GetTree().CreateTimer(fireRate), "timeout");
		canShoot = true;
	}
	public void UpdateBulletCount(int b)
	{
		GetNode<Label>("HUD/BulletCount").Text = "Number of Pewpew Left: " + b.ToString();
	}
}
