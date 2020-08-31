using Godot;
using System;

public class ShotgunBullet : Area2D
{
    public Vector2 dir { get ; set ; } 
    public int speed { get ; set ; } 

    public override void _Process(float delta)
    {
        Position += dir * delta * speed;
    }

    private void _on_VisibilityNotifier2D_screen_exited()
    {     
        this.QueueFree();
    }
}
