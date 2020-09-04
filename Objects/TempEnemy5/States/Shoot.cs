using System;
using System.Threading.Tasks;
using Godot;

namespace Debugmancer.Objects.TempEnemy5.States
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
			_host.GetNode<Node2D>("BulletSpawn").Rotation = new Vector2(_target.Position.x - _host.Position.x,
				_target.Position.y - _host.Position.y).Angle();
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

		public async void ShootTimer_timeout()
		{

			if (_shots++ < 20)
				SpawnBullet();
			else
			{
				await Task.Delay(700);
				EmitSignal(nameof(State.Finished), "Teleport");
			}
		}

		private void SpawnBullet()
		{
			var bullet = (EnemyBullet2)_bulletScene.Instance();
			bullet.Speed = 135;
			bullet.Rotation = _host.GetNode<Node2D>("BulletSpawn").Rotation;
			bullet.GlobalPosition = _host.Position;
			bullet.Direction = Vector2.Right.Rotated(_host.GetNode<Position2D>("BulletSpawn/Position2D").RotationDegrees);
			_host.GetParent().AddChild(bullet);
			_host.GetNode<Node2D>("BulletSpawn").Rotate(_shots < 6 ? .5f : -.5f);
		}
	}
}
