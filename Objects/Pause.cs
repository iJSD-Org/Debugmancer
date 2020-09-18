using Godot;

namespace Debugmancer.Objects
{
	public class Pause : Control
	{
		
		public static bool IsPaused = false;
		public override void _Input(InputEvent @event)
		{
			if(Input.IsActionJustPressed("pause") && !ModuleDropper.IsInModule)
			{
				GetTree().Paused = !GetTree().Paused;
				IsPaused = Visible = GetTree().Paused;
			}
		}
		public void _on_ResumeButton_button_up()
		{
			GetTree().Paused = false;
			Visible = false;
			IsPaused = false;
			
		}
		public void _on_QuitButton_button_up()
		{
			GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Play(12.47f);
			GetTree().Paused = false;
			GetTree().ChangeScene("res://Levels/Main Menu.tscn");
		}
	}
}
