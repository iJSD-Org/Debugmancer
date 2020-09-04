using System;
using Godot;

namespace Debugmancer.Objects
{
	public class ScreenShake : Camera2D
	{
		[Export] public float Amplitude = 16;

		public enum States
		{
			Idle, Shaking
		}

		public States State = States.Idle;
		public Vector2 StartPosition;

		private readonly Random _random = new Random();


		public override void _Ready()
		{
			SetAsToplevel(true);
			SetProcess(false);
			StartPosition = GlobalPosition;
		}
        public override void _PhysicsProcess(float delta)
        {
            Position = GetParent().GetNode<KinematicBody2D>("Player").Position;
        }
        
		public void StartShake()
		{
			ChangeState(States.Shaking);
		}

		public void ChangeState(States newState)
		{
			switch (newState)
			{
				case States.Idle:
					Position = StartPosition;
					SetProcess(false);
					break;
				case States.Shaking:
					StartPosition = Position;
					SetProcess(true);
					GetNode<Timer>("ShakeTimer").Start();
					break;
			}

			State = newState;
		}

		public override void _Process(float delta)
		{
			Vector2 shakeOffset = new Vector2(
				(float) (_random.NextDouble() * (Amplitude - -Amplitude) + -Amplitude), 
				(float)(_random.NextDouble() * (Amplitude - -Amplitude) + -Amplitude));
			Position = GlobalPosition + shakeOffset;
		}

		public void ShakeTimer_timeout()
		{
			ChangeState(States.Idle);

		}
	}
}
