using Godot;

namespace Debugmancer.Levels
{
	public partial class Start : Node2D
	{
		private int _scene = 1;

		public override void _EnterTree()
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
			Input.MouseMode = Input.MouseModeEnum.Hidden;
		}

		public override void _ExitTree()
		{
			DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.AlwaysOnTop, false);
			DisplayServer.WindowGetSize();
		}

		private void _on_Timer_timeout()
		{
			GetNode<Timer>("Timer").Stop();
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
			if (_scene == 2)
			{
				_scene++;
			}
		}

		private void _on_AnimationPlayer_finished(string animName)
		{
			if (animName == "FadeOut" && _scene <= 1)
			{
				GetNode<Timer>("Timer").Start();
				GetNode<Sprite2D>("Sprite1").Hide();
				GetNode<Sprite2D>("Sprite2").Show();
				GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
				_scene++;
			}
			else if (_scene == 3)
			{
				GetTree().ChangeSceneToFile("res://Levels/Cutscene.tscn");
			}
		}

	}
}
