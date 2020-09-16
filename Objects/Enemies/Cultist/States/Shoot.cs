using Debugmancer.Objects.Bullets;
using Godot;

namespace Debugmancer.Objects.Enemies.Cultist.States
{
	public class Shoot : State
	{
		private int _shots;
		private KinematicBody2D _target;
		private KinematicBody2D _host;
		private PackedScene _bulletScene;

		public void Init(KinematicBody2D target, PackedScene bulletScene)
		{
			_target = target;
			_bulletScene = bulletScene;
		}

		public override void Enter(KinematicBody2D host)
		{
			_shots = 0;
			_host = host;
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Chase");
			_host.GetNode<Node2D>("BulletSpawn").Rotation = new Vector2(_target.Position.x - _host.Position.x,
				_target.Position.y - _host.Position.y).Angle();
			GetNode<Timer>("ShootTimer").Start();
		}

		public override void Exit(KinematicBody2D host)
		{
			// Nothing to do here
		}

		public override void HandleInput(KinematicBody2D host, InputEvent @event)
		{
			// Nothing to do here
		}

		public override void Update(KinematicBody2D host, float delta)
		{
			// Nothing to do here
		}

		public void ShootTimer_timeout()
		{
			if (_shots++ < 20)
				SpawnBullet();
			else
			{
				GetNode<Timer>("ShootTimer").Stop();
				GetNode<Timer>("CooldownTimer").Start();
			}
		}

		public void CooldownTimer_timeout()
		{
			GetNode<Timer>("CooldownTimer").Stop();
			EmitSignal(nameof(Finished), "Teleport");
		}

		private void SpawnBullet()
		{
			var bullet = (EnemyBullet2)_bulletScene.Instance();
			bullet.Speed = 85;
			bullet.Rotation = _host.GetNode<Node2D>("BulletSpawn").Rotation;
			bullet.GlobalPosition = _host.Position;
			bullet.Direction = Vector2.Right.Rotated(_host.GetNode<Position2D>("BulletSpawn/Position2D").RotationDegrees);
			_host.GetParent().AddChild(bullet);
			_host.GetNode<Node2D>("BulletSpawn").Rotate(_shots < 6 ? .5f : -.5f);
		}
	}
}
