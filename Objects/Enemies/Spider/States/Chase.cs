using System;
using Debugmancer.Objects.Player;
using Godot;

namespace Debugmancer.Objects.Enemies.Spider.States
{
	public partial class Chase : State
	{
		[Export] public int Speed = 90;
		[Export] public int SpaceOutChance = 6;
		private bool _canChase = true;
		private Vector2 _direction;
		private Player.Entity _target;
		private Timer _chaseTimer;
		private readonly Random _random = new Random();
		public override void _Ready()
		{
			_chaseTimer = GetNode<Timer>("ChaseTimer");
		}

		public void Init(Player.Entity target)
		{
			_target = target;
			_chaseTimer.Start();
		}

		public override void Enter(CharacterBody2D host)
		{
			host.GetNode<AnimationPlayer>("AnimationPlayer").Play("Chase");
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
			if (_canChase) ChaseTarget(host);
			host.Velocity = _direction * Speed;
			host.MoveAndSlide();
		}

		private void _on_ChaseTimer_timeout()
		{
			_canChase = GetParent().GetParent().GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").IsOnScreen();

			_chaseTimer.Stop();
			if (_canChase)
			{
				string state = _random.Next(1, 10) > SpaceOutChance ? "Chase" : "Idle";
				EmitSignal(nameof(Finished), state);
			}
			else
			{
				EmitSignal(nameof(Finished), "Idle");
			}
		}
		private void ChaseTarget(CharacterBody2D host)
		{
			RayCast2D look = host.GetNode<RayCast2D>("RayCast2D");
			if (_target != null) look.TargetPosition = _target.Position - host.Position;
			look.ForceRaycastUpdate();

			// if we can see the target, chase it
			if (!look.IsColliding() || ((Node)look.GetCollider()).IsInGroup("player"))
			{
				((Entity)host).GetNode<Timer>("ShootTimer").Paused = false;
				_direction = look.TargetPosition.Normalized();
			}
			// or chase the first scent we see
			else
			{
				((Entity)host).GetNode<Timer>("ShootTimer").Paused = true;
				foreach (Scent scent in _target.ScentTrail)
				{
					look.TargetPosition = scent.Position - host.Position;
					look.ForceRaycastUpdate();

					if (!look.IsColliding() || ((Node)look.GetCollider()).IsInGroup("player"))
					{
						_direction = look.TargetPosition.Normalized();
						break;
					}
				}
			}
		}
	}
}
