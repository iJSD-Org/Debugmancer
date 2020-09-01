using Godot;
using System;

namespace Debugmancer.Objects
{
	public class Gun : Sprite
	{
		[Export] public PackedScene Bullet = ResourceLoader.Load("res://Objects/Bullet.tscn") as PackedScene;
		[Export] public PackedScene CritBullet = ResourceLoader.Load("res://Objects/CritBullet.tscn") as PackedScene;

		[Export] public float BulletSpeed = 1000f;
		[Export] public int BulletCount = 50;
		[Export] public float FireRate = 0.2f;
		private bool _canShoot = true;

		public override void _Process(float delta)
		{
			Rotation = GetParent<KinematicBody2D>().GetAngleTo(GetGlobalMousePosition());
		}
		public void Fire()
		{
			if (_canShoot && BulletCount > 0)
			{
				Random random = new Random();
				RigidBody2D bulletInstance = random.Next(1, 100) > 10 ? (RigidBody2D)CritBullet.Instance() : (RigidBody2D)Bullet.Instance();
				bulletInstance.Position = GetNode<Node2D>("GunPoint").GlobalPosition;
				bulletInstance.Rotation = Rotation;
				bulletInstance.ApplyImpulse(new Vector2(0, 0), new Vector2(BulletSpeed, 0).Rotated(Rotation));
				GetTree().Root.AddChild(bulletInstance);
				BulletCount--;
				ShootTimer();

				//debug
				GetNode<Label>("HUD/BulletCount").Text = $"Number of Bullets Left: {BulletCount}";
			}
		}
		public async void ShootTimer()
		{
			_canShoot = false;
			await ToSignal(GetTree().CreateTimer(FireRate), "timeout");
			_canShoot = true;
		}
	}
}
