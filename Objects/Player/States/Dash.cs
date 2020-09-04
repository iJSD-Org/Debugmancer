using System;
using System.Threading.Tasks;
using Godot;

namespace Debugmancer.Objects.Player.States
{
	public class Dash : State
	{
		private readonly Vector2 _dashSpeed = new Vector2(215, 215);
		private Vector2 _inputDirection;
		public override async void Enter(KinematicBody2D host)
		{
			host.GetNode<Timer>("DashTimer").Start();
			host.GetNode<CollisionShape2D>("Hitbox/CollisionShape2D").Disabled = true;
			if (!host.GetNode<Sprite>("Sprite").FlipH)
			{
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Dash");
			}
			else
			{
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Dash_Flipped");
			}
			_inputDirection = new Vector2(
				Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left")),
				Convert.ToInt32(Input.IsActionPressed("move_down")) - Convert.ToInt32(Input.IsActionPressed("move_up")));
			// Visual feedback for dashing
			host.Modulate = Color.Color8(100, 100, 100);
			await Task.Delay(300);
			host.Modulate = new Color(1, 1, 1);
		}

		public override void Exit(KinematicBody2D host)
		{
			host.GetNode<CollisionShape2D>("Hitbox/CollisionShape2D").Disabled = false;
			host.GetNode<Timer>("DashTimer").Stop();
		}

		public override void HandleInput(KinematicBody2D host, InputEvent @event)
		{
			// Nothing to do here
		}

		private void _on_DashTimer_timeout()
		{			
			EmitSignal(nameof(Finished), "Move");
		}

		public override void Update(KinematicBody2D host, float delta)
		{
			host.MoveAndSlide(_inputDirection.Normalized() * _dashSpeed, Vector2.Zero);
		}
	}
}
