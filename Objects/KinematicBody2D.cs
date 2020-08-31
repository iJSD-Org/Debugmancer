using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
    private Vector2 inputVector = Vector2.Zero;
    private const int SPEED = 250;
    private bool isWalking = false;

    public override void _Process(float delta)
    {
        inputVector = GetInput();
        Move();
    }

    private Vector2 GetInput()
    {
        Vector2 velocity = new Vector2();
        velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        if (velocity.x != 0 || velocity.y != 0)
        {
            isWalking = true;
        }
        return velocity * SPEED;
    }

    private void Move()
    {
        inputVector = MoveAndSlide(inputVector);
        isWalking = false;
    }
}
