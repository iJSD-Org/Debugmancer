using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
	[Export] public int Speed = 150;
	private Vector2 _inputVector = Vector2.Zero;
	private bool _isDashing;
	private bool _canDash = true;

	public override void _Process(float delta)
	{
		Sprite weapon = GetNode<Sprite>("Gun");
		if (Math.Abs(weapon.Rotation) < 90 * (Math.PI / 180)) TurnRight();
		else if (Math.Abs(weapon.Rotation) >= 90 * (Math.PI / 180)) TurnLeft();
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_isDashing)
			MoveAndSlide(_inputVector.Normalized() * new Vector2(1000, 1000));
		else
			_inputVector = MoveAndSlide(GetInput());
	}
	private Vector2 GetInput()
	{
		Vector2 velocity = new Vector2
		{
			x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
			y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
		};
		if (Input.IsActionJustPressed("dash") && _canDash) Dash();

		return velocity * Speed;
	}
	private void TurnLeft()
	{
		Sprite weapon = GetNode<Sprite>("Gun");
		weapon.Position = new Vector2(-Mathf.Abs(weapon.Position.x), weapon.Position.y);
		weapon.FlipV = true;
		Sprite player = GetNode<Sprite>("Sprite");
		player.FlipH = true;
	}
	private void TurnRight()
	{
		Sprite weapon = GetNode<Sprite>("Gun");
		weapon.Position = new Vector2(Mathf.Abs(weapon.Position.x), weapon.Position.y);
		weapon.FlipV = false;
		Sprite player = GetNode<Sprite>("Sprite");
		player.FlipH = false;
	}
	public async void Dash()
	{
		_isDashing = true;
		// Visual feedback for dashing
		Modulate = Color.Color8(100, 100, 100);
		await ToSignal(GetTree().CreateTimer(.07f), "timeout");
		Modulate = new Color(1, 1, 1);
		_isDashing = false;
		DashTimer();
	}

	public async void DashTimer()
	{
		_canDash = false;
		await ToSignal(GetTree().CreateTimer(3), "timeout");
		_canDash = true;
	}
}
