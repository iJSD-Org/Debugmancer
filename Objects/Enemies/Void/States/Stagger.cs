using System.Threading.Tasks;
using Godot;

namespace Debugmancer.Objects.Enemies.Void.States
{
	public partial class Stagger : State
	{
		public override async void Enter(CharacterBody2D host)
		{
			// TODO: Switch to Stagger Animation
			host.Modulate = Color.Color8(255, 0, 0);
			await Task.Delay(150);
			host.Modulate = new Color(1, 1, 1);
			EmitSignal(nameof(Finished), "Previous");
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
			// Do nothing here, being in this state causes the stagger itself
		}
	}
}
