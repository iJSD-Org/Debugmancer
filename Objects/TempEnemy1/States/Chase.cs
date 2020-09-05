using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects.TempEnemy1.States
{
	public class Chase : State
	{
		[Export] public int Speed = 60;
		private Vector2 _direction;
		private Player.Entity _target;

		public void Init(Player.Entity target)
		{
			_target = target;
		}

		public override void Enter(KinematicBody2D host)
		{
			// TODO: Switch to Chase Animation
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
			ChaseTarget(host);
			host.MoveAndSlide(_direction * Speed);
		}

		private void ChaseTarget(KinematicBody2D host)
		{

			RayCast2D look = host.GetNode<RayCast2D>("RayCast2D");
			look.CastTo = _target.Position - host.Position;
			look.ForceRaycastUpdate();

			// if we can see the target, chase it
			if (!look.IsColliding() || ((Node)look.GetCollider()).IsInGroup("player"))
			{
				((Entity)host).GetNode<Timer>("ShootTimer").Paused = false;
				_direction = look.CastTo.Normalized();
			}
			// or chase the first scent we see
			else
			{
				((Entity) host).GetNode<Timer>("ShootTimer").Paused = true;
				foreach (Scent scent in _target.ScentTrail)
				{
					look.CastTo = scent.Position - host.Position;
					look.ForceRaycastUpdate();

					if (!look.IsColliding() || ((Node)look.GetCollider()).IsInGroup("player"))
					{
						_direction = look.CastTo.Normalized();
						break;
					}
				}
			}
		}
	}
}
