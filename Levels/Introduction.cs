using Godot;

public class Introduction : Node2D
{
	private int _scene = 1;
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
		GetNode<Timer>("Timer").Start();
	}

	private void _on_Timer_timeout()
	{
		GetNode<Timer>("Timer").Stop();
		GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
		if(_scene == 2)
		{
			_scene++;
		}
	}

	private void _on_AnimationPlayer_finished(string anim_name)
	{
		if(anim_name == "FadeOut" && _scene <= 1)
		{
			GetNode<Timer>("Timer").Start();
			GetNode<Sprite>("Sprite1").Hide();
			GetNode<Sprite>("Sprite2").Show();
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
			_scene++;
		}
		else if(_scene == 3)
		{
			GetTree().ChangeScene("res://Levels/Main Menu.tscn");
		}

	}
	
}
