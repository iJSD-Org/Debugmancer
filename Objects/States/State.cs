using Godot;

namespace Debugmancer.Objects.States
{
	public abstract class State : Node
	{
		public abstract void Enter(KinematicBody2D host);
		public abstract void Exit(KinematicBody2D host);
		public abstract string HandleInput(KinematicBody2D host, InputEvent @event);
		public abstract string Update(KinematicBody2D host, float delta);
	}
}
