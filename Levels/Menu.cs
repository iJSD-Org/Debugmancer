using Godot;

namespace Debugmancer.Levels
{
	public partial class Menu : Control
	{
		public override void _EnterTree()
		{
			Engine.TimeScale = 1;
			GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").PitchScale = 1;
			GetNode<AnimationPlayer>("MenuAnimPlayer").Play("Transition");
		}

		private void _button_hovered()
		{
			GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
		}
		private void _on_Start_pressed()
		{
			if (((RichPresence)GetNode("/root/RichPresence")).Client.CurrentUser != null)
			{
				GetNode<ColorRect>("ColorRect").Show();
				GetNode<AnimationPlayer>("MenuAnimPlayer").Play("FadeOut");
			}
			else
			{
				GetNode<AcceptDialog>("ConnectionFailedDialog").PopupCentered();
				GD.Print("Show");
			}
		}

		private void _on_Settings_pressed()
		{
			GetNode<Window>("SettingsDialog").PopupCentered();
		}

		private void _on_Leaderboard_pressed()
		{
			GetTree().ChangeSceneToFile("res://Levels/Leaderboard.tscn");
		}

		private void _on_Quit_pressed()
		{
			GetTree().Quit();
		}

		private void _on_MenuAnimPlayer_animation_finished(string animName)
		{
			if (animName == "FadeIn")
			{
				GetNode<ColorRect>("ColorRect").Hide();
			}
			if (animName == "FadeOut")
			{
				GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Stop();
				GetTree().ChangeSceneToFile("res://Levels/Arena.tscn");
			}
			if (animName == "Transition")
			{
				GetNode<AnimationPlayer>("MenuAnimPlayer").Play("FadeIn");
			}
		}
	}
}
