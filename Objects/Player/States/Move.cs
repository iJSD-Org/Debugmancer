using System;
using Godot;

namespace Debugmancer.Objects.Player.States
{
	public partial class Move : State
	{
		[Export] public int Speed;
		public Vector2 Velocity;
		public override void Enter(CharacterBody2D host)
		{
			Speed = 0;
			Velocity = new Vector2();
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Walk");
		}

		public override void Exit(CharacterBody2D host)
		{
			// Nothing to do here
		}

		public override void HandleInput(CharacterBody2D host, InputEvent @event)
		{
			if (@event.IsActionPressed("dash"))
			{
				EmitSignal(nameof(Finished), "Dash");
			}
		}

		public override void Update(CharacterBody2D host, float delta)
		{
			Vector2 inputDirection = new Vector2(
				Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
				Convert.ToInt32(Input.IsActionPressed("move_down")) - Convert.ToInt32(Input.IsActionPressed("move_up")));
			if (inputDirection == Vector2.Zero)
			{
				EmitSignal(nameof(Finished), "Idle");
			}

			Speed = 95;

			MoveObject(host, Speed, inputDirection);
		}

		public KinematicCollision2D MoveObject(CharacterBody2D host, int speed, Vector2 direction)
		{
			Vector2 velocity = direction.Normalized() * speed;
			host.Velocity = velocity;
			host.MoveAndSlide();
			
			return host.GetSlideCollisionCount() == 0 ? null : host.GetSlideCollision(0);
		}
	}
}
