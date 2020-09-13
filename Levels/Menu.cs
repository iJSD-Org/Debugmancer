using Godot;
using Debugmancer.Objects;

namespace Debugmancer.Levels
{
	public class Menu : Control
	{
		public override void _EnterTree()
		{
			Engine.TimeScale = 1;
			GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").PitchScale = 1;
			GetNode<AnimationPlayer>("MenuAnimPlayer").Play("Transition");
		}

		public override void _ExitTree()
		{
			GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Stop();
		}

		private void _on_Start_pressed()
		{
			GetNode<ColorRect>("ColorRect").Show();
			GetNode<AnimationPlayer>("MenuAnimPlayer").Play("FadeOut");
		}

		private void _on_Quit_pressed()
		{
			GetTree().Quit();
		}

		private void _on_MenuAnimPlayer_finished(string animName)
		{
			if (animName == "FadeIn")
			{
				GetNode<ColorRect>("ColorRect").Hide();
			}
			if (animName == "FadeOut")
			{
				GetTree().ChangeScene("res://Levels/TestArena.tscn");
			}
			if (animName == "Transition")
			{
				GetNode<AnimationPlayer>("MenuAnimPlayer").Play("FadeIn");
			}
		}
	}
}
