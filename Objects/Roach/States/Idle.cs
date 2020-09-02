using Godot;

namespace Debugmancer.Objects.Roach.States
{
	public class Idle : State
	{
		public override void Enter(KinematicBody2D host)
		{
			// TODO: Switch to Idle Animation
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
			GD.Print("Idle");
			if (GetTree().Root.HasNode("Player"))
			{
				EmitSignal(nameof(Finished),"Chase");
			}
		}
	}
}
