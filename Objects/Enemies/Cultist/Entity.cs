using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Debugmancer.Objects.TempEnemy5.States;
using Godot;

namespace Debugmancer.Objects.TempEnemy5
{
	public class Entity : KinematicBody2D
	{
		[Signal]
		public delegate void StateChanged();
		
		public State CurrentState;
		public readonly Dictionary<string, Node> StatesMap = new Dictionary<string, Node>();

		private KinematicBody2D _player;

		public override void _Ready()
		{
			StatesMap.Add("Shoot", GetNode("States/Shoot"));
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Chase");
			StatesMap.Add("Teleport", GetNode("States/Teleport"));

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished), this, nameof(ChangeState));
			}

			_player = GetParent().GetNode("Player") as KinematicBody2D;
			GetNode<Node2D>("BulletSpawn").Rotation = new Vector2(_player.Position.x - Position.x, _player.Position.y - Position.y).Angle();

			GetNode("Health").Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));

			CurrentState = (State)GetNode("States/Shoot");
			ChangeState("Shoot");
		}

		public override void _PhysicsProcess(float delta)
		{
			CurrentState.Update(this, delta);
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
				((Shoot)CurrentState).Init(GetParent().GetNode<KinematicBody2D>("Player"), (PackedScene)ResourceLoader.Load("res://Objects/Bullets/EnemyBullet2.tscn"));
			}

			if (stateName == "Teleport")
			{
				((Teleport)CurrentState).Init(GetParent().GetNode<KinematicBody2D>("Player"));
			}

			CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		public async void OnHealthChanged(int health)
		{
			Modulate = Color.ColorN("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
			{
				Globals.score += Math.Ceiling(150 * Globals.scoreMultiplier);
				GetParent().GetNode<KinematicBody2D>("Player").GetNode<Label>("HUD/Score").Text = $"Score:{Globals.score}";
				QueueFree();
			}
		}

		public void _on_Hitbox_body_entered(Area2D body)
		{
			Health health = (Health)GetNode("Health");
			if (body.IsInGroup("playerBullet")) 
			{
				if(health.CurrentHealth - Globals.playerDamage > 0) health.Damage(Globals.playerDamage);
				else health.Damage(Globals.playerDamage - (health.CurrentHealth - Globals.playerDamage));
			}
			
			if (body.IsInGroup("playerCritBullet"))
			{
				if(health.CurrentHealth - (Globals.playerDamage * 2) > 0)
				{
					ChangeState("Teleport");
					health.Damage(Globals.playerDamage * 2);
				}
				else health.Damage(Globals.playerDamage - (health.CurrentHealth - (Globals.playerDamage * 2)));
			}
		}
	}
}
