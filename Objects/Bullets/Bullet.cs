using Godot;

namespace Debugmancer.Objects.Bullets
{
	public partial class Bullet : RigidBody2D
	{
		public void _on_Bullet_body_entered(Node body)
		{
			if (!body.IsInGroup("player"))  QueueFree();
		}
	}
}






