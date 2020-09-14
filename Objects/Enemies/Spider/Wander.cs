using Godot;
using System;


namespace Debugmancer.Objects.Enemies.Spider.States
{   
    public class Wander : State
    {
        [Export] public int Speed = 40;
        private Vector2 _dir = new Vector2();
        private Timer _wanderTimer;
        private RayCast2D _look;
        private Random _random = new Random();
        public override void _Ready()
        {
             _wanderTimer = GetNode<Timer>("WanderTimer");
        }
        public void Init(Player.Entity target)
		{

		}

		public override void Enter(KinematicBody2D host)
		{
			_wanderTimer.Start();
            _dir.x = _random.Next(-50,50);
            _dir.y = _random.Next(-50,50);
            _look = host.GetNode<RayCast2D>("RayCast2D");
            GD.Print(_look.CastTo + " " + _dir);
            _look.CastTo = _dir;
        }
		public override void Exit(KinematicBody2D host)
		{
			// Nothing to do here
		}

		public override void HandleInput(KinematicBody2D host, InputEvent @event)
		{
			// Nothing to do here
		}

		public override void Update(KinematicBody2D host, float delta)
		{
            _look.ForceRaycastUpdate();
            if (_look.IsColliding())
            {
                if (_look.GetCollisionPoint() - host.GlobalPosition <= new Vector2(20,20))  _dir = Vector2.Zero;
            }
            host.MoveAndSlide(_dir.Normalized() * Speed);
		}

        private void _on_WanderTimer_timeout()
        {
            _wanderTimer.Stop();
            EmitSignal(nameof(Finished), "Idle");
        }

    }   
}
