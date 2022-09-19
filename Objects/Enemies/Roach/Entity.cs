using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Debugmancer.Objects.Enemies.Roach.States;
using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects.Enemies.Roach
{
	public partial class Entity : CharacterBody2D
	{
		[Signal]
		public delegate void StateChangedEventHandler();

		public State CurrentState;
		public Stack<State> StateStack = new();
		public readonly Dictionary<string, Node> StatesMap = new();
		private CharacterBody2D _player;

		public override void _Ready()
		{
			StatesMap.Add("Chase", GetNode("States/Chase"));
			StatesMap.Add("Stagger", GetNode("States/Stagger"));
			StatesMap.Add("Idle", GetNode("States/Idle"));
			StatesMap.Add("Wander", GetNode("States/Wander"));

			CurrentState = (Idle)GetNode("States/Idle");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished),new Callable(this,nameof(ChangeState)));
			}

			GetNode("Health").Connect(nameof(Health.HealthChanged),new Callable(this,nameof(OnHealthChanged)));

			StateStack.Push((State)StatesMap["Idle"]);
			ChangeState("Idle");
		}

		public override void _PhysicsProcess(double delta)
		{
			CurrentState.Update(this, (float)delta);
		}

		private void ChangeState(string stateName)
		{
			CurrentState.Exit(this);
			if (stateName == "Previous")
			{
				StateStack.Pop();
			}
			else if (stateName == "Dead")
			{
				QueueFree();
				return;
			}
			else if (stateName == "Stagger")
			{
				StateStack.Push((State)StatesMap[stateName]);
			}
			else
			{
				StateStack.Pop();
				StateStack.Push((State)StatesMap[stateName]);
			}

			CurrentState = StateStack.Peek();

			// Pass target to Chase State
			if (stateName == "Chase")
			{
				((Chase)CurrentState).Init((Player.Entity)_player);
			}

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		private void _on_VisibilityNotifier2D_screen_entered()
		{
			_player = GetParent().GetNode<CharacterBody2D>("Player");
			ChangeState("Chase");
		}
		private void _on_VisibilityNotifier2D_screen_exited()
		{
			GetNode<Timer>("States/Chase/ChaseTimer").Stop();		
			ChangeState("Idle");
		}

		public async void OnHealthChanged(int health)
		{
			Modulate = new Color("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
			{
				Globals.Score += (int)Math.Ceiling(35 * Globals.ScoreMultiplier);
				GetParent().GetNode<CharacterBody2D>("Player").GetNode<Label>("HUD/Score").Text = $"Score: {Globals.Score}";
				ChangeState("Dead");
			}
		}

		public void Hitbox_BodyEntered(Node2D body)
		{
			Health health = (Health)GetNode("Health");
			if (body.IsInGroup("playerBullet")) 
			{
				if(health.CurrentHealth - Globals.PlayerDamage > 0) health.Damage(Globals.PlayerDamage);
				else health.Damage(Globals.PlayerDamage - (health.CurrentHealth - Globals.PlayerDamage));
			}

			if (body.IsInGroup("playerCritBullet"))
			{
				if(health.CurrentHealth - (Globals.PlayerDamage * 2) > 0)
				{
					ChangeState("Stagger");
					health.Damage(Globals.PlayerDamage * 2);
				}
				else health.Damage(Globals.PlayerDamage - (health.CurrentHealth - (Globals.PlayerDamage * 2)));
			}
		}
	}
}
