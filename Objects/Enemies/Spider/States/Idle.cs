using Godot;
using System;


namespace Debugmancer.Objects.Enemies.Spider.States
{   
    public class Idle : State
    {
         [Export] public int WanderChance = 5;
        private Timer _idleTimer; 
        private Random _random = new Random();
        private bool _chase = false;
        public override void _Ready()
        {
            _idleTimer = GetNode<Timer>("IdleTimer");
        }

        public void Init(Player.Entity target)
		{
        
		}

		public override void Enter(KinematicBody2D host)
		{
           _idleTimer.Start();
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
			host.MoveAndSlide(Vector2.Zero);
		}

        private void _on_IdleTimer_timeout()
        {
			if (GetParent().GetParent().GetNode<VisibilityNotifier2D>("VisibilityNotifier2D").IsOnScreen())
			{
				GD.Print("ON SCREEN");
				_chase = true;
			}
			else
			{
				_chase = false;
			}
            _idleTimer.Stop();
            if (!_chase || _random.Next(1,10) < WanderChance) EmitSignal(nameof(Finished), "Wander");
            else EmitSignal(nameof(Finished), "Chase");
        }
    }   
}
