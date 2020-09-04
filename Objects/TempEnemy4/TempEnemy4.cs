using System.Threading.Tasks;
using Godot;

namespace Debugmancer.Objects.TempEnemy4
{
	public class TempEnemy4 : KinematicBody2D
	{
		private readonly PackedScene _shotgunScene = (PackedScene)GD.Load("res://Objects/Bullets/ShotgunBullet.tscn");
		private KinematicBody2D _player;
		private bool _canShoot = true;
		private readonly Timer _shootTimer = new Timer();

		public override void _Ready()
		{
			_player = GetParent().GetNode("Player") as KinematicBody2D;
			_shootTimer.WaitTime = 1.0f;
			_shootTimer.Connect("timeout", this, "on_timer_timeout");
			AddChild(_shootTimer);
			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));
		}

		public override void _Process(float delta)
		{
			if (_canShoot)
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
			var bullet = (ShotgunBullet)_shotgunScene.Instance();
			bullet.Speed = 200;
			bullet.Position = Position;
			bullet.Rotation = (_player.Position - GlobalPosition).Angle();
			bullet.Direction = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Normalized();
			GetParent().AddChild(bullet);
			_shootTimer.Start();
			_canShoot = false;
		}

		public async void OnHealthChanged(int health)
		{

			Modulate = Color.ColorN("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
				QueueFree();
		}

		public void _on_Hitbox_body_entered(Area2D body)
		{
			Health health = (Health)GetNode("Health");
			if (body.IsInGroup("playerBullet")) health.Damage(1);

			if (body.IsInGroup("playerCritBullet")) health.Damage(2);
		}
	}
}
