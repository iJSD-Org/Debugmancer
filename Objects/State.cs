using Godot;

namespace Debugmancer.Objects
{
	public abstract partial class State : Node
	{
		[Signal]
		public delegate void FinishedEventHandler(string nextStateName);
		public abstract void Enter(CharacterBody2D host);
		public abstract void Exit(CharacterBody2D host);
		public abstract void HandleInput(CharacterBody2D host, InputEvent @event);
		public abstract void Update(CharacterBody2D host, float delta);
	}
}
