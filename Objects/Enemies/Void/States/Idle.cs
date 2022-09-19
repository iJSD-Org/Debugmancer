using Godot;
using System;


namespace Debugmancer.Objects.Enemies.Void.States
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
			//he chillin
		}

		private void _on_IdleTimer_timeout()
		{
			_chase = GetParent().GetParent().GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").IsOnScreen();

			_idleTimer.Stop();
			if (!_chase || _random.Next(1, 10) < WanderChance) EmitSignal(nameof(Finished), "Wander");
			else EmitSignal(nameof(Finished), "Chase");
		}

		private void _on_VisibilityNotifier2D_screen_entered()
		{
			_chase = true;
		}

		private void _on_VisibilityNotifier2D_screen_exited()
		{
			_chase = false;
		}
	}
}
