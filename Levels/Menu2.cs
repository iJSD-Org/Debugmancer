using Godot;
using System;

public class Menu2 : Control
{
    public override void _Ready()
    {
        base._Ready();
        GetNode<AnimationPlayer>("MenuAnimPlayer").Play("Transition");
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
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

	private void _on_MenuAnimPlayer_finished(string anim_name)
	{
		if (anim_name == "FadeIn")
		{
			GetNode<ColorRect>("ColorRect").Hide();
		}
		if (anim_name == "FadeOut")
		{
			GetTree().ChangeScene("res://Levels/TestArena.tscn");
		}
		if (anim_name == "Transition")
		{
			GetNode<AnimationPlayer>("MenuAnimPlayer").Play("FadeIn");
		}
	}
}
