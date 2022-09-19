using Godot;

namespace Debugmancer.Objects
{
	public partial class ConnectionFailedDialog : AcceptDialog
	{
		private void ConnectionFailedDialog_AboutToShow()
		{
			GetNode<AudioStreamPlayer>("Alert").Play();
		}
	}
}
