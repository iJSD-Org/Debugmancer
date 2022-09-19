using Godot;

namespace Debugmancer.Objects.Bullets
{
	public partial class EnemyBullet2 : Area2D
	{
		public Vector2 Direction { get; set; }
		public int Speed { get; set; }

		public override void _Process(double delta)
		{
			Position += new Vector2(1, 0).Rotated(Rotation) * Speed * (float)delta;
		}

		private void _on_VisibilityNotifier2D_screen_exited()
		{
			QueueFree();
		}

		public void _on_EnemyBullet_body_entered(Node2D body)
		{
			if (!body.IsInGroup("enemy") && !body.IsInGroup("playerBullet") ) QueueFree();
		}
	}
}
