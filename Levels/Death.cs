using Godot;
using System;
using Debugmancer.Objects;

public class Death : Control
{

    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
        GetNode<Label>("Score").Text = $"You scored : {Globals.score}";
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (Input.IsActionPressed("E"))
        {
             GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
        }
    }

    public void _on_AnimationPlayer_finished(string anim_name)
    {
        if (anim_name == "FadeOut")
        {
            GetTree().ChangeScene("res://Levels/Second Main Menu.tscn");
        }
    }

}
