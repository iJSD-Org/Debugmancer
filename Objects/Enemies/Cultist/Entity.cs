using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Debugmancer.Objects.Enemies.Cultist.States;
using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects.Enemies.Cultist
{
	public partial class Entity : CharacterBody2D
	{
		[Signal]
		public delegate void StateChangedEventHandler();
		
		public State CurrentState;
		public readonly Dictionary<string, Node> StatesMap = new();

		private CharacterBody2D _player;

		public override void _Ready()
		{
			StatesMap.Add("Shoot", GetNode("States/Shoot"));
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Chase");
			StatesMap.Add("Teleport", GetNode("States/Teleport"));

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished),new Callable(this,nameof(ChangeState)));
			}

			_player = GetParent().GetNode("Player") as CharacterBody2D;
			GetNode<Node2D>("BulletSpawn").Rotation = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Angle();

			GetNode("Health").Connect(nameof(Health.HealthChanged),new Callable(this,nameof(OnHealthChanged)));

			CurrentState = (State)GetNode("States/Shoot");
			ChangeState("Shoot");
		}

		public override void _PhysicsProcess(double delta)
		{
			CurrentState.Update(this, (float)delta);
		}

		private void ChangeState(string stateName)
		{
			GD.Print(stateName);
			CurrentState.Exit(this);

			if (stateName == "Dead")
			{
				QueueFree();
				return;
			}
			
			CurrentState = (State)StatesMap[stateName];

			if (stateName == "Shoot")
			{
				((Shoot)CurrentState).Init(GetParent().GetNode<CharacterBody2D>("Player"), (PackedScene)ResourceLoader.Load("res://Objects/Bullets/EnemyBullet2.tscn"));
			}

			if (stateName == "Teleport")
			{
				((Teleport)CurrentState).Init(GetParent().GetNode<CharacterBody2D>("Player"));
			}

			CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		public async void OnHealthChanged(int health)
		{
			Modulate = new Color("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
			{
				Globals.Score += (int)Math.Ceiling(200 * Globals.ScoreMultiplier);
				GetParent().GetNode<CharacterBody2D>("Player").GetNode<Label>("HUD/Score").Text = $"Score: {Globals.Score}";
				QueueFree();
			}
		}

		public void _on_Hitbox_body_entered(Node2D body)
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
					ChangeState("Teleport");
					health.Damage(Globals.PlayerDamage * 2);
				}
				else health.Damage(Globals.PlayerDamage - (health.CurrentHealth - (Globals.PlayerDamage * 2)));
			}
		}
	}
}
