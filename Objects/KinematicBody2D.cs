using Godot;
using System;

public class KinematicBody2D : Godot.KinematicBody2D
{
    [Export] public int Speed = 150;
    [Export] public float FireRate = 0.2f;
    [Export] public int BulletCount = 50;
    [Export] public float BulletSpeed = 1000f;
    [Export] public PackedScene Bullet = ResourceLoader.Load("res://Objects/Bullet.tscn") as PackedScene;
    private Vector2 _inputVector = Vector2.Zero;
    private bool _isWalking;
    private bool _isDashing = false;
    private bool _canDash = true;
    private bool _canShoot = true;

    public override void _Process(float delta)
    {
        // TODO: Future stuff here
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_isDashing)
        {
            MoveAndSlide(_inputVector.Normalized() * new Vector2(1000, 1000));
        }
        else
        {
            _inputVector = MoveAndSlide(GetInput());
        }
    }
    private Vector2 GetInput()
    {
        Vector2 velocity = new Vector2();
        LookAt(GetGlobalMousePosition());
        velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        if (Input.IsActionJustPressed("dash") && _canDash) Dash();

        _isWalking = velocity.x != 0 || velocity.y != 0;

        if (Input.IsActionPressed("click") && _canShoot && BulletCount > 0)
        {
            RigidBody2D bulletInstance = (RigidBody2D)Bullet.Instance();
            bulletInstance.Position = GetNode<Node2D>("Gun").GlobalPosition;
            bulletInstance.RotationDegrees = RotationDegrees;
            bulletInstance.ApplyImpulse(new Vector2(0, 0), new Vector2(BulletSpeed, 0).Rotated(Rotation));
            GetTree().Root.AddChild(bulletInstance);
            BulletCount--;
            GetNode<Label>("HUD/BulletCount").Text = $"Number of Bullets Left: {BulletCount}";
            ShootTimer();
        }
        return velocity * Speed;
    }

    public async void Dash()
    {
        _isDashing = true;
        // Visual feedback that its dashing
        Modulate = Color.Color8(100, 100, 100);
        await ToSignal(GetTree().CreateTimer(.07f), "timeout");
        Modulate = new Color(1, 1, 1);
        _isDashing = false;
        DashTimer();
        RandomNumberGenerator gen = new RandomNumberGenerator();
        float waitTime = gen.RandfRange(.15f, 2.0f);

    }

    public async void DashTimer()
    {
        _canDash = false;
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        _canDash = true;
    }
    public async void ShootTimer()
    {
        _canShoot = false;
        await ToSignal(GetTree().CreateTimer(FireRate), "timeout");
        _canShoot = true;
    }
}
