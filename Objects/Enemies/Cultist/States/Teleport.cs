using Godot;

namespace Debugmancer.Objects.Enemies.Cultist.States
{
	public partial class Teleport : State
	{
		private CharacterBody2D _target;
		private CharacterBody2D _host;

		public void Init(CharacterBody2D target)
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

		public override void Enter(CharacterBody2D host)
		{
			_host = host;
			host.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Disappear");
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
			// Nothing to do here
		}
	}
}
