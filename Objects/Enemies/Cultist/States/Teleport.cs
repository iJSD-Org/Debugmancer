using Godot;

namespace Debugmancer.Objects.Enemies.Cultist.States
{
	public class Teleport : State
	{
		private KinematicBody2D _target;
		private KinematicBody2D _host;

		public void Init(KinematicBody2D target)
		{
			_target = target;
		}

		private void _on_AnimationPlayer_finished(string animName)
		{
			if (animName == "Disappear")
			{
				_host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Appear");
				_host.Position = ((Player.Entity)_target).ScentTrail[0].Position;
			}
			if (animName == "Appear")
			{
				EmitSignal(nameof(Finished), "Shoot");
			}
		}

		public override void Enter(KinematicBody2D host)
		{
			_host = host;
			host.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Disappear");
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
