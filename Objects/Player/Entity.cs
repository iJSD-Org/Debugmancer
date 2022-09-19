using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Timer = System.Timers.Timer;

namespace Debugmancer.Objects.Player
{
	public partial class Entity : CharacterBody2D
	{
		[Signal]
		public delegate void StateChangedEventHandler();

		public PackedScene ScentScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/Scent.tscn");

		public List<Scent> ScentTrail = new();

		public State CurrentState;
		public Stack<State> StateStack = new();
		private readonly Timer _dashCooldownTimer = new();
		public readonly Dictionary<string, Node> StatesMap = new();
		private bool _isRecover;
		
		public override void _Ready()
		{
			GetNode<TextureProgressBar>("HUD/VBoxContainer/Health").MaxValue = ((Health)GetNode("Health")).MaxHealth;
			GetNode<TextureProgressBar>("HUD/VBoxContainer/Health").Value = ((Health)GetNode("Health")).CurrentHealth;
			StatesMap.Add("Idle", GetNode("States/Idle"));
			StatesMap.Add("Move", GetNode("States/Move"));
			StatesMap.Add("Dash", GetNode("States/Dash"));

			CurrentState = (State)GetNode("States/Idle");

			foreach (Node state in StatesMap.Values)
			{
				state.Connect(nameof(State.Finished),new Callable(this,nameof(ChangeState)));
			}

			_dashCooldownTimer.AutoReset = false;
			_dashCooldownTimer.Enabled = false;
			_dashCooldownTimer.Interval = 1000;

			StateStack.Push((State)StatesMap["Idle"]);
			ChangeState("Idle");
			GetNode<Label>("HUD/User").Text =
				((RichPresence) GetNode("/root/RichPresence")).Client.CurrentUser.ToString();
		}

		public override void _Process(double delta)
		{
			Sprite2D weapon = GetNode<Sprite2D>("Gun");
			if (Math.Abs(weapon.Rotation) < 90 * (Math.PI / 180)) TurnRight();
			else if (Math.Abs(weapon.Rotation) >= 90 * (Math.PI / 180)) TurnLeft();
		}


		public override void _PhysicsProcess(double delta)
		{
			CurrentState.Update(this, (float)delta);
		}

		public override void _Input(InputEvent @event)
		{
			// Firing is the weapon"s responsibility so the weapon should handle it
			if (@event.IsActionPressed("click"))
			{
				((Gun)GetNode<Sprite2D>("Gun")).Fire();
				return;
			}
			CurrentState.HandleInput(this, @event);
		}

		private void TurnLeft()
		{
			Sprite2D weapon = GetNode<Sprite2D>("Gun");
			weapon.Position = new Vector2(-Mathf.Abs(weapon.Position.x), weapon.Position.y);
			weapon.FlipV = true;
			Sprite2D player = GetNode<Sprite2D>("Sprite2D");
			player.FlipH = true;
		}
		private void TurnRight()
		{
			Sprite2D weapon = GetNode<Sprite2D>("Gun");
			weapon.Position = new Vector2(Mathf.Abs(weapon.Position.x), weapon.Position.y);
			weapon.FlipV = false;
			Sprite2D player = GetNode<Sprite2D>("Sprite2D");
			player.FlipH = false;
		}

		#region Signal Receivers
		private void ChangeState(string stateName)
		{
			CurrentState.Exit(this);
			if (stateName == "Previous")
			{
				StateStack.Pop();
			}
			else if (stateName == "Dash")
			{
				if (!_dashCooldownTimer.Enabled && Globals.CanDash && Globals.Energy - 5 > 0)
				{
					Globals.Energy -= 10;
					GetNode<TextureProgressBar>("HUD/VBoxContainer/Energy").Value = Globals.Energy;
					_dashCooldownTimer.Start();
					StateStack.Push((State)StatesMap[stateName]);
				}

			}
			else if (stateName == "Dead")
			{
				Globals.IsDying = true;
				Engine.TimeScale = 0.4f;
				GetNode<AnimationPlayer>("FadePlayer").Play("FadeOut");
				return;
			}
			else
			{
				StateStack.Pop();
				StateStack.Push((State)StatesMap[stateName]);
			}

			CurrentState = StateStack.Peek();

			// We don"t want to reinitialize the state if we"re going back to the previous state
			if (stateName != "Previous")
				CurrentState.Enter(this);

			EmitSignal(nameof(StateChanged), CurrentState.Name);
		}

		public async void OnHealthChanged(int health)
		{
			if (!Globals.IsRecover)((Camera)GetNode<Camera2D>("Camera")).StartShake();
			GetNode<TextureProgressBar>("HUD/VBoxContainer/Health").Value = health;
			Modulate = _isRecover ? new Color("Green") : new Color("Red");
			await Task.Delay(100);
			Modulate = new Color(1, 1, 1);
			if (health == 0)
				ChangeState("Dead");
		}

		public void AddScent()
		{
			Scent scent = (Scent)ScentScene.Instantiate();
			scent.Position = Position;
			GetTree().Root.AddChild(scent);

			scent.Init(this);
			ScentTrail.Add(scent);
		}

		public void Hitbox_BodyEntered(CharacterBody2D body)
		{
			if (body.IsInGroup("roach")) {
				_isRecover = false;
				((Health)GetNode("Health")).Damage(5);
			}
			else if (body.IsInGroup("enemy")) {
				_isRecover = false;
				((Health)GetNode("Health")).Damage(1);
			}
		}

		public void BodyTimer_timeout()
		{
			List<CharacterBody2D> bodies =
				GetNode<Area2D>("Hitbox").GetOverlappingBodies().OfType<CharacterBody2D>().ToList();
			if (bodies.Any(b => b.IsInGroup("enemy")))
			{
				_isRecover = false;
				((Health)GetNode("Health")).Damage(2);
			}
		}

		public void _on_RecoverTimer_timeout()
		{
			_isRecover = true;
			((Health)GetNode("Health")).Recover(1);
		}
		public void Hitbox_AreaEntered(Area2D area)
		{
			if (area.IsInGroup("shotgunBullet")) {
				_isRecover = false;
				((Health)GetNode("Health")).Damage(4);
			}
			if (area.IsInGroup("enemyBullet")) {
				_isRecover = false;
				((Health)GetNode("Health")).Damage(2);
			}
		}

		private void _on_FadePlayer_finished(string animName)
		{
			if (animName == "FadeOut")
			{
				Engine.TimeScale = 1f;
				GetTree().ChangeSceneToFile("res://Levels/Death screen.tscn");
			}
		}
		#endregion
	}
}
