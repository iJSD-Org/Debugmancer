using Godot;

namespace Debugmancer.Objects.Player
{
	public class Scent : Node2D
	{
		private Entity _player;

		public void Init(Entity host)
		{
			_player = host;
		}

		public void ScentExpired()
		{
			_player.ScentTrail.Remove(this);
			QueueFree();
		}
	}
}
