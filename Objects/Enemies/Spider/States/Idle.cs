using Godot;
using System;


namespace Debugmancer.Objects.Enemies.Spider.States
{   
    public partial class Idle : State
    {
         [Export] public int WanderChance = 5;
        private Timer _idleTimer; 
        private readonly Random _random = new Random();
        private bool _chase;
        public override void _Ready()
        {
            _idleTimer = GetNode<Timer>("IdleTimer");
        }

        public override void Enter(CharacterBody2D host)
		{
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
           _idleTimer.Start();
		}

		public override void Exit(CharacterBody2D host)
		{
			// Nothing to do here
		}

		public override void HandleInput(CharacterBody2D host, InputEvent @event)
		{
			// Nothing to do here
		}

		public override void Update(CharacterBody2D host, float delta)
		{
			host.Velocity = Vector2.Zero;
			host.MoveAndSlide();
		}

        private void _on_IdleTimer_timeout()
        {
			if (GetParent().GetParent().GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").IsOnScreen())
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
