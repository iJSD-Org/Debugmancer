using Godot;

namespace Debugmancer.Objects.Bullets
{
	public partial class ShotgunBullet : Area2D
	{
		public Vector2 Direction { get; set; }
		public int Speed { get; set; }

		public override void _Process(double delta)
		{
			Position += Direction * (float)delta * Speed;
		}

		private void _on_VisibilityNotifier2D_screen_exited()
		{
			QueueFree();
		}
		public void _on_ShotgunBullet_body_entered(Node2D body)
		{
			if (!body.IsInGroup("enemy") && !body.IsInGroup("playerBullet") ) QueueFree();		
		}
	}
}
