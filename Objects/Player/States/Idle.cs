using System;
using Godot;

namespace Debugmancer.Objects.Player.States
{
	public partial class Idle : State
	{

		public override void Enter(CharacterBody2D host)
		{
			 host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
		}

		public override void Exit(CharacterBody2D host)
		{
		
		}

		public override void HandleInput(CharacterBody2D host, InputEvent @event)
		{
			// Nothing to do here
		}

		public override void Update(CharacterBody2D host, float delta)
		{
			Vector2 inputDirection = new Vector2(
				Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
				Convert.ToInt32(Input.IsActionPressed("move_down")) - Convert.ToInt32(Input.IsActionPressed("move_up")));

			if (inputDirection != Vector2.Zero) EmitSignal(nameof(Finished), "Move");
		}
	}
}
