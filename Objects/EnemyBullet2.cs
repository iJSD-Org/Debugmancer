using Godot;
using System;

public class EnemyBullet2 : Area2D
{
	public Vector2 Direction { get; set; }
	public int Speed { get; set; }

	public override void _Process(float delta)
	{
		Position += new Vector2(1, 0).Rotated(Rotation) * Speed * delta;
	}

	private void _on_VisibilityNotifier2D_screen_exited()
	{
		QueueFree();
	}
}
