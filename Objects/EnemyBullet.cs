using Godot;
using System;

public class EnemyBullet : Area2D
{
	public Vector2 Direction { get; set; }
	public int Speed { get; set; }

	public override void _Process(float delta)
	{
		Position += Direction * delta * Speed;
	}

	private void _on_VisibilityNotifier2D_screen_exited()
	{
		QueueFree();
	}
}