using Godot;
using System;
using Debugmancer.Objects.Player;

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
			if (_canShoot && Globals.Energy - 8 > 0)
			{
				Random random = new Random();
				RigidBody2D bulletInstance = random.Next(1, 10) > Globals.CritChance ? (RigidBody2D)Bullet.Instance() : (RigidBody2D)CritBullet.Instance();
				bulletInstance.Position = GetNode<Node2D>("GunPoint").GlobalPosition;
				bulletInstance.Rotation = Rotation;
				bulletInstance.ApplyImpulse(new Vector2(0, 0), new Vector2(BulletSpeed, 0).Rotated(Rotation));
				GetTree().Root.AddChild(bulletInstance);
				ReduceEnergy();
				ShootTimer();
			}
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Globals.Energy;
		}
		public void _on_RegenTimer_timeout()
		{
			if (Globals.Energy < MaxEnergy)
			{
				if(Globals.Energy > (MaxEnergy *.8)) Globals.Energy += 16;
				if(Globals.Energy >= (MaxEnergy * .5)) Globals.Energy += 13;
				if(Globals.Energy < (MaxEnergy * .5)) Globals.Energy += 8;
				if(Globals.Energy > MaxEnergy) Globals.Energy = MaxEnergy;
				GD.Print(Globals.Energy);
			}
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Globals.Energy;
		}
		public void ReduceEnergy()
		{
			if(Globals.Energy > (MaxEnergy *.8)) Globals.Energy -= 3;
			if(Globals.Energy >= (MaxEnergy * .5)) Globals.Energy -= 5;
			if(Globals.Energy < (MaxEnergy * .5)) Globals.Energy -= 8;
			GetParent().GetNode<TextureProgress>("HUD/VBoxContainer/Energy").Value = Globals.Energy;
		}
		public async void ShootTimer()
		{
			_canShoot = false;
			await ToSignal(GetTree().CreateTimer(FireRate), "timeout");
			_canShoot = true;
		}
	}
}
