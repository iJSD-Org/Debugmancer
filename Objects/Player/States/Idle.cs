using System;
using Godot;

namespace Debugmancer.Objects.Player.States
{
	public class Idle : State
	{

		public override void Enter(KinematicBody2D host)
		{
			 host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
		}

		public override void Exit(KinematicBody2D host)
		{
		
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

			if (inputDirection != Vector2.Zero) EmitSignal(nameof(Finished), "Move");
		}
	}
}
