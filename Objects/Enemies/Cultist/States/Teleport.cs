using Godot;

namespace Debugmancer.Objects.Enemies.Cultist.States
{
	public class Teleport : State
	{
		private KinematicBody2D _target;

		public void Init(KinematicBody2D target)
		{
			_target = target;
		}

		public override void Enter(KinematicBody2D host)
		{
			host.Position = ((Player.Entity)_target).ScentTrail[0].Position;
			EmitSignal(nameof(Finished), "Shoot");
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
			// Nothing to do here
		}
	}
}
