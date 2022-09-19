using Godot;
using System;


namespace Debugmancer.Objects.Enemies.Void.States
{
	public partial class Wander : State
	{
		[Export] public int Speed = 40;
		private Vector2 _dir;
		private Timer _wanderTimer;

		private readonly Random _random = new Random();

		public override void _Ready()
		{
			_wanderTimer = GetNode<Timer>("WanderTimer");
		}

		public override void Enter(CharacterBody2D host)
		{
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Chase");
			_wanderTimer.Start();
			_dir.x = _random.Next(-50, 50);
			_dir.y = _random.Next(-50, 50);
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
			host.Velocity = _dir.Normalized() * Speed;
			host.MoveAndSlide();
		}

		private void _on_WanderTimer_timeout()
		{
			_wanderTimer.Stop();
			EmitSignal(nameof(Finished), "Idle");
		}

	}
}
