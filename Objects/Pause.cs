using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects
{
	public partial class Pause : Control
	{		
		public override void _Input(InputEvent @event)
		{
			if(Input.IsActionJustPressed("pause"))
			{
				GetTree().Paused = !GetTree().Paused;
				Visible = GetTree().Paused;
				GetNode("../../ModuleDropper").SetProcessInput(!GetTree().Paused);
			}
		}
		public void _on_ResumeButton_button_up()
		{
			GetNode("../../ModuleDropper").SetProcessInput(true);
			GetTree().Paused = false;
			Visible = false;	
		}
		public void _on_QuitButton_button_up()
		{
			Globals.ResetValues();
			GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Play(12.47f);
			GetTree().Paused = false;
			GetTree().ChangeSceneToFile("res://Levels/Main Menu.tscn");
		}
	}
}
