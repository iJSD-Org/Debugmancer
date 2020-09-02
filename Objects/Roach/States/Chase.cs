using Godot;

namespace Debugmancer.Objects.Roach.States
{
	public class Chase : State
	{
		[Export] public int Speed;
		private Vector2 _direction;

		public override void Enter(KinematicBody2D host)
		{
			// TODO: Switch to Chase Animation
			ChaseTarget(host);
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
			host.MoveAndSlide(_direction * Speed);
		}

		private void ChaseTarget(KinematicBody2D host)
		{
			Player.Entity player = (Player.Entity)GetTree().Root.GetNode<KinematicBody2D>("Player");

			RayCast2D look = host.GetNode<RayCast2D>("RayCast2D");
			look.CastTo = player.Position - host.Position;
			look.ForceRaycastUpdate();
			
			// if we can see the target, chase it
			if (!look.IsColliding())
			{
				_direction = look.CastTo.Normalized();
			}
			// or chase the first scent we see
			else
			{
				foreach (Scent scent in player.ScentTrail)
				{
					look.CastTo = scent.Position - host.Position;
					look.ForceRaycastUpdate();

					if (!look.IsColliding())
					{
						_direction = look.CastTo.Normalized();
						break;
					}
				}
			}
		}

	}
}