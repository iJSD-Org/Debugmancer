using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Levels
{
	public class Death : Control
	{

		public override void _Ready()
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
			GetNode<Label>("Score").Text = $"You scored : {Globals.Score}";
		}

		public override void _Process(float delta)
		{
			base._Process(delta);

			if (Input.IsActionPressed("E"))
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
			}
		}

		public void _on_AnimationPlayer_finished(string animName)
		{
			if (animName == "FadeOut")
			{
				GetNode<AudioStreamPlayer>("/root/BackgroundMusic/MenuMusic").Play(12.47f);
				GetTree().ChangeScene("res://Levels/Main Menu.tscn");
			}
		}

	}
}
