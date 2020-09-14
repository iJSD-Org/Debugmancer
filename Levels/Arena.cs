using Godot;

namespace Debugmancer.Levels
{
	public class Arena : Node
	{
		private readonly Resource _arrow = ResourceLoader.Load("res://Assets/Sprites/Crosshair.png");

		public override void _EnterTree()
		{
			Input.SetCustomMouseCursor(_arrow);
		}

		public override void _ExitTree()
		{
			Input.SetCustomMouseCursor(null);
		}
	}
}
