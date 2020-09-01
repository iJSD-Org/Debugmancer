using System;
using Godot;

namespace Debugmancer.Objects.States.Player
{
	public class Move : State
	{
		[Export] public int Speed;
		public Vector2 Velocity;
		public override void Enter(KinematicBody2D host)
		{
			Speed = 0;
			Velocity = new Vector2();
			// TODO: Play/Switch to Move Animation here
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
			Vector2 inputDirection = new Vector2(
				Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
				Convert.ToInt32(Input.IsActionPressed("move_down")) - Convert.ToInt32(Input.IsActionPressed("move_up")));
			if (inputDirection == Vector2.Zero)
			{
				EmitSignal(nameof(Finished), "Idle");
			}

			Speed = 100;

			MoveObject(host, Speed, inputDirection);
		}

		public KinematicCollision2D MoveObject(KinematicBody2D host, int speed, Vector2 direction)
		{
			Vector2 velocity = direction.Normalized() * speed;
			host.MoveAndSlide(velocity, Vector2.Zero);
			if (host.GetSlideCount() == 0)
				return null;
			return host.GetSlideCollision(0);
		}
	}
}
