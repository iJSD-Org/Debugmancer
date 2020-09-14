using Godot;

namespace Debugmancer.Objects
{
	public class ConnectionFailedDialog : AcceptDialog
	{
		private void ConnectionFailedDialog_AboutToShow()
		{
			GetNode<AudioStreamPlayer>("Alert").Play();
		}
	}
}
