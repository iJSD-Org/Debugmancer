using System.Threading.Tasks;
using Godot;

namespace Debugmancer.Objects.Roach.States
{
	public class Stagger : State
	{
		public override async void Enter(KinematicBody2D host)
		{
			// TODO: Switch to Stagger Animation
			host.Modulate = Color.Color8(255, 0, 0);
			await Task.Delay(150);
			host.Modulate = new Color(1, 1, 1);
			EmitSignal(nameof(Finished), "Previous");
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
			// TODO: Stagger here
		}
	}
}
