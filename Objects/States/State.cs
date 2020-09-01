using Godot;

namespace Debugmancer.Objects.States
{
	public abstract class State : Node
	{
		[Signal]
		public delegate void Finished(string nextStateName);
		public abstract void Enter(KinematicBody2D host);
		public abstract void Exit(KinematicBody2D host);
		public abstract void HandleInput(KinematicBody2D host, InputEvent @event);
		public abstract void Update(KinematicBody2D host, float delta);
	}
}
