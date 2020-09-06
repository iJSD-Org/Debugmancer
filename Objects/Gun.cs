using Godot;
using System;

namespace Debugmancer.Objects
{
	public class Gun : Sprite
	{
		[Export] public PackedScene Bullet = ResourceLoader.Load<PackedScene>("res://Objects/Bullets/Bullet.tscn");
		[Export] public PackedScene CritBullet = ResourceLoader.Load<PackedScene>("res://Objects/Bullets/CritBullet.tscn");

		[Export] public float BulletSpeed = 600f;
		[Export] public int BulletCount = 1000;
		[Export] public float FireRate = 0.2f;
		private bool _canShoot = true;
		[Export] public int Energy = 100; 
		[Export] public int MaxEnergy = 100; 
		public override void _Process(float delta)
		{
			Rotation = GetParent<KinematicBody2D>().GetAngleTo(GetGlobalMousePosition());
		}
		public void Fire()
		{
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Energy;
			if (_canShoot && Energy - 8 > 0)
			{
				Random random = new Random();
				RigidBody2D bulletInstance = random.Next(1, 10) > 1 ? (RigidBody2D)Bullet.Instance() : (RigidBody2D)CritBullet.Instance();
				bulletInstance.Position = GetNode<Node2D>("GunPoint").GlobalPosition;
				bulletInstance.Rotation = Rotation;
				bulletInstance.ApplyImpulse(new Vector2(0, 0), new Vector2(BulletSpeed, 0).Rotated(Rotation));
				GetTree().Root.AddChild(bulletInstance);
				BulletCount--;
				ReduceEnergy();
				ShootTimer();
			}
		}
		public void _on_RegenTimer_timeout()
		{
			if (Energy < MaxEnergy)
			{
				if(Energy > (MaxEnergy *.8)) Energy += 16;
				if(Energy >= (MaxEnergy * .5)) Energy += 13;
				if(Energy < (MaxEnergy * .5)) Energy += 8;
				if(Energy > MaxEnergy) Energy = MaxEnergy;
				GD.Print(Energy);
			}
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Energy;
		}
		public void ReduceEnergy()
		{
			if(Energy > (MaxEnergy *.8)) Energy -= 3;
			if(Energy >= (MaxEnergy * .5)) Energy -= 5;
			if(Energy < (MaxEnergy * .5)) Energy -= 8;
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Energy;
		}
		public async void ShootTimer()
		{
			_canShoot = false;
			await ToSignal(GetTree().CreateTimer(FireRate), "timeout");
			_canShoot = true;
		}
	}
}
