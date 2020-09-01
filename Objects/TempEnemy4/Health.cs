using Godot;
using System;

namespace Debugmancer.Objects.TempEnemy4
{
	public class Health : Node
	{
		[Signal]
		public delegate void HealthChanged();

		public int CurrentHealth;
		[Export] public int MaxHealth = 100;

		public override void _Ready()
		{
			CurrentHealth = MaxHealth;
		}

		public void Damage(int amount)
		{
			CurrentHealth -= amount;
			if (CurrentHealth <= 0)
			{
				CurrentHealth = 0;
				EmitSignal(nameof(HealthChanged));
			}
			EmitSignal(nameof(HealthChanged), CurrentHealth);
		}

		public void Recover(int amount)
		{
			CurrentHealth -= amount;
			if (CurrentHealth < 0)
			{
				CurrentHealth = 0;
			}
			EmitSignal(nameof(HealthChanged), CurrentHealth);
		}
	}
}
