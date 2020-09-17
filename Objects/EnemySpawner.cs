using System;
using System.Collections.Generic;
using System.Linq;
using Debugmancer.Objects.Player;
using Godot;
using Godot.Collections;

namespace Debugmancer.Objects
{
	public class EnemySpawner : Node
	{
		[Export] public Array<PackedScene> Enemies = new Array<PackedScene>();
		[Export] public double EnemyMultiplier = 1;
		private readonly Random _random = new Random();
		private double _maxEnemies;
		private double _enemies = 0;
		private float _timerCoolDown = 35;
		public override void _Ready()
		{
			_maxEnemies = Math.Ceiling(3 * EnemyMultiplier);
			GetNode<Timer>("SpawnTimer").Start();
		}

		public void SpawnTimer_timeout()
		{
			int enemies = GetChildren().Cast<Node>().Count(c => c.IsInGroup("enemy"));

			GetNode<Timer>("SpawnTimer").Stop();
			List<Vector2> areas = new List<Vector2> {
				new Vector2(_random.Next(-385, -55), _random.Next(450, 550)), //leftmost space
				new Vector2(_random.Next(50, 540), _random.Next(125, 270)), //top
				new Vector2(_random.Next(50, 1090), _random.Next(500, 590)), //center bottom
				new Vector2(_random.Next(50, 750), _random.Next(335, 440)), //center top
				new Vector2(_random.Next(50, 150), _random.Next(650, 835)), //bottom left
				new Vector2(_random.Next(260, 370), _random.Next(650, 835)), //bottom right
				new Vector2(_random.Next(910, 1120), _random.Next(330, 380)),//rightmost space
			};

			Vector2 enemyPosition = areas[_random.Next(areas.Count)];	
			
			if(_enemies <= _maxEnemies && enemies < 20)
			{		
				_enemies++;
				GetNode<Timer>("SpawnTimer").WaitTime = 0.7f;
				KinematicBody2D enemy = (KinematicBody2D)Enemies[_random.Next(Enemies.Count)].Instance();
				enemy.Position = enemyPosition;
				float a = _random.Next(0,1) == 0 ? enemyPosition.x += 15 : enemyPosition.y += 15;
				GetParent().AddChild(enemy); 
				GetNode<Timer>("SpawnTimer").Start();
			}

			else
			{
				_enemies = 0;
				GetNode<Timer>("SpawnTimer").WaitTime = _timerCoolDown;
				GetNode<Timer>("SpawnTimer").Start();
				_maxEnemies = Math.Ceiling(3 * EnemyMultiplier);
				EnemyMultiplier += .3;
				Globals.ScoreMultiplier += .3;
				_timerCoolDown -= 0.2f;
			}	
		}
	}
}
