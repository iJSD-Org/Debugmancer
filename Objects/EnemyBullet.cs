using Godot;

namespace Debugmancer.Objects
{
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
		
		public void _on_EnemyBullet_body_entered(Area2D body)
		{
			if (!body.IsInGroup("enemies")) QueueFree();
		}
	}
}
