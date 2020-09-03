using Godot;

namespace Debugmancer.Objects.Player
{
	public class Scent : Node2D
	{
		private Player _player;

		public void Init(Player host)
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
