using Godot;

namespace Debugmancer.Objects.Player
{
	public class Scent : Node2D
	{
		public Entity Entity;

		public void RemoveScent()
		{
			Entity.ScentTrail.Remove(this);
			QueueFree();
		}
	}
}
