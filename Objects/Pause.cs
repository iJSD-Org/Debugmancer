using Godot;

namespace Debugmancer.Objects
{
	public class Pause : Control
	{
		public override void _Input(InputEvent @event)
		{
			if(Input.IsActionJustPressed("pause"))
			{
				GetTree().Paused = !GetTree().Paused;
				Visible = GetTree().Paused;
			}
		}
		public void _on_ResumeButton_button_up()
		{
			GetTree().Paused = false;
			Visible = false;
		}
		public void _on_QuitButton_button_up()
		{
		
		}
	}
}
