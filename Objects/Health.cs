using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects
{
	public class Health : Node
	{
		[Signal]
		public delegate void HealthChanged(int health);

		public int CurrentHealth;
		[Export] public int MaxHealth = 100;

		public override void _Ready()
		{
			CurrentHealth = MaxHealth;
		}

		public void Damage(int amount)
		{
			Globals.IsRecover = false;
			if(CurrentHealth <= 0) return;
			CurrentHealth -= amount;
			if (CurrentHealth <= 0)
			{
				CurrentHealth = 0;
				EmitSignal(nameof(HealthChanged));
			}

			EmitSignal(nameof(HealthChanged), CurrentHealth);

			//debug
			GD.Print($"{GetPath()} took {amount} damage. Health: {CurrentHealth} / {MaxHealth}");
		}

		public void Recover(int amount)
		{
			Globals.IsRecover = true;
			CurrentHealth += amount;
			if (CurrentHealth > MaxHealth)
			{
				CurrentHealth = MaxHealth;
			}
			EmitSignal(nameof(HealthChanged), CurrentHealth);
			//debug
			GD.Print($"{GetPath()} recovered {amount} health. Health: {CurrentHealth} / {MaxHealth}");
		}
	}
}
