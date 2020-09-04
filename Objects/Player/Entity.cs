using System;
using System.Collections.Generic;
using Godot;
using Timer = System.Timers.Timer;

namespace Debugmancer.Objects.Player
{
	public class StateMachine : KinematicBody2D
	{
		[Signal]
		public delegate void StateChanged();

		public State CurrentState;
		public Stack<State> StateStack = new Stack<State>();
		private readonly Timer _dashCooldownTimer = new Timer();
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();

		public override void _Ready()
		{
			StatesMap.Add("Idle", GetNode("State/Idle"));
			StatesMap.Add("Move", GetNode("State/Move"));
			StatesMap.Add("Dash", GetNode("State/Dash"));

			CurrentState = (State)GetNode("State/Idle");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished), this, nameof(ChangeState));
			}

			_dashCooldownTimer.AutoReset = false;
			_dashCooldownTimer.Enabled = false;
			_dashCooldownTimer.Interval = 500;

			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));

			StateStack.Push((State)StatesMap["Idle"]);
			ChangeState("Idle");
		}

		public override void _Process(float delta)
		{
			Sprite weapon = GetNode<Sprite>("Gun");
			if (Math.Abs(weapon.Rotation) < 90 * (Math.PI / 180)) TurnRight();
			else if (Math.Abs(weapon.Rotation) >= 90 * (Math.PI / 180)) TurnLeft();
		}

		public override void _PhysicsProcess(float delta)
		{
			CurrentState.Update(this, delta);
		}

		public override void _Input(InputEvent @event)
		{
			// Firing is the weapon"s responsibility so the weapon should handle it
			if (@event.IsActionPressed("click"))
			{
				((Gun)GetNode<Sprite>("Gun")).Fire();
				return;
			}
			CurrentState.HandleInput(this, @event);
		}

		private void ChangeState(string stateName)
		{
			CurrentState.Exit(this);
			GD.Print(!_dashCooldownTimer.Enabled);
			if (stateName == "Previous")
			{
				StateStack.Pop();
			}
			else if (stateName == "Dash")
			{
				if (!_dashCooldownTimer.Enabled)
				{
					_dashCooldownTimer.Start();
					StateStack.Push((State)StatesMap[stateName]);
				}
			}
			else if (stateName == "Dead")
			{
				QueueFree();
				return;
			}
			else
			{
				StateStack.Pop();
				StateStack.Push((State)StatesMap[stateName]);
			}

			CurrentState = StateStack.Peek();
			GD.Print(CurrentState.Name);

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		private void TurnLeft()
		{
			Sprite weapon = GetNode<Sprite>("Gun");
			weapon.Position = new Vector2(-Mathf.Abs(weapon.Position.x), weapon.Position.y);
			weapon.FlipV = true;
			Sprite player = GetNode<Sprite>("Sprite");
			player.FlipH = true;
		}
		private void TurnRight()
		{
			Sprite weapon = GetNode<Sprite>("Gun");
			weapon.Position = new Vector2(Mathf.Abs(weapon.Position.x), weapon.Position.y);
			weapon.FlipV = false;
			Sprite player = GetNode<Sprite>("Sprite");
			player.FlipH = false;
		}

		public void OnHealthChanged(int health)
		{
			if (health == 0)
				ChangeState("Dead");
		}

		public void _on_Hitbox_body_entered(Area2D body)
		{
			if (body.IsInGroup("enemyBullet"))
				((Health)GetNode("Health")).Damage(1);
		}
		public void _on_Hitbox_area_entered(Area2D area)
		{
			Health playerHealth = (Health) GetNode("Health");
			if (area.IsInGroup("shotgunBullet")) playerHealth.Damage(5);
			if (area.IsInGroup("enemyBullet")) playerHealth.Damage(1);
		}
	}
}
