using System;
using System.Threading.Tasks;
using Godot;

namespace Debugmancer.Objects.States.Player
{
	public class Dash : State
	{
		private readonly Vector2 _dashSpeed = new Vector2(1000, 1000);
		private Vector2 _inputDirection;
		public override async void Enter(KinematicBody2D host)
		{
			_inputDirection = new Vector2(
				Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
				Convert.ToInt32(Input.IsActionPressed("move_down")) - Convert.ToInt32(Input.IsActionPressed("move_up")));
			// Visual feedback for dashing
			host.Modulate = Color.Color8(100, 100, 100);
			await Task.Delay(70);
			host.Modulate = new Color(1, 1, 1);
			EmitSignal(nameof(Finished),"Move");
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
			host.MoveAndSlide(_inputDirection.Normalized() * _dashSpeed,Vector2.Zero);
		}
	}
}
