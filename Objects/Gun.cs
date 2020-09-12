using Godot;
using System;

namespace Debugmancer.Objects
{
	public class Gun : Sprite
	{
		[Export] public PackedScene Bullet = ResourceLoader.Load<PackedScene>("res://Objects/Bullets/Bullet.tscn");
		[Export] public PackedScene CritBullet = ResourceLoader.Load<PackedScene>("res://Objects/Bullets/CritBullet.tscn");

		[Export] public float BulletSpeed = 600f;
		[Export] public float FireRate = 0.2f;
		private bool _canShoot = true;
		//[Export] public int Energy = 100; 
		[Export] public int MaxEnergy = 100; 
		public override void _Process(float delta)
		{
			Rotation = GetParent<KinematicBody2D>().GetAngleTo(GetGlobalMousePosition());
		}
		public void Fire()
		{
			if (_canShoot && Globals.energy - 8 > 0)
			{
				Random random = new Random();
				RigidBody2D bulletInstance = random.Next(1, 10) > Globals.critChance ? (RigidBody2D)Bullet.Instance() : (RigidBody2D)CritBullet.Instance();
				bulletInstance.Position = GetNode<Node2D>("GunPoint").GlobalPosition;
				bulletInstance.Rotation = Rotation;
				bulletInstance.ApplyImpulse(new Vector2(0, 0), new Vector2(BulletSpeed, 0).Rotated(Rotation));
				GetTree().Root.AddChild(bulletInstance);
				ReduceEnergy();
				ShootTimer();
			}
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Globals.energy;
		}
		public void _on_RegenTimer_timeout()
		{
			if (Globals.energy < MaxEnergy)
			{
				if(Globals.energy > (MaxEnergy *.8)) Globals.energy += 16;
				if(Globals.energy >= (MaxEnergy * .5)) Globals.energy += 13;
				if(Globals.energy < (MaxEnergy * .5)) Globals.energy += 8;
				if(Globals.energy > MaxEnergy) Globals.energy = MaxEnergy;
				GD.Print(Globals.energy);
			}
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Globals.energy;
		}
		public void ReduceEnergy()
		{
			if(Globals.energy > (MaxEnergy *.8)) Globals.energy -= 3;
			if(Globals.energy >= (MaxEnergy * .5)) Globals.energy -= 5;
			if(Globals.energy < (MaxEnergy * .5)) Globals.energy -= 8;
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Globals.energy;
		}
		public async void ShootTimer()
		{
			_canShoot = false;
			await ToSignal(GetTree().CreateTimer(FireRate), "timeout");
			_canShoot = true;
		}
	}
}
