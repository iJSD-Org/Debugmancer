using Godot;

namespace Debugmancer
{
	public class FullscreenToggle : Node
	{
		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed("toggle_fullscreen") && !OS.IsWindowAlwaysOnTop())
			{
				OS.WindowFullscreen = !OS.WindowFullscreen;
			}
		}
	}
}
