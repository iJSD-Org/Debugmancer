using System;
using System.Collections.Generic;
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
			GetNode<Timer>("SpawnTimer").Stop();
			List<Vector2> areas = new List<Vector2> {
				new Vector2(_random.Next(-425, -10), _random.Next(425, 600)), //leftmost space
				new Vector2(_random.Next(30, 105), _random.Next(100, 270)), //top
				new Vector2(_random.Next(30, 1190), _random.Next(500, 590)), //center bottom
				new Vector2(_random.Next(30, 850), _random.Next(335, 440)), //center top
				new Vector2(_random.Next(30, 190), _random.Next(650, 935)), //bottom left
				new Vector2(_random.Next(260, 420), _random.Next(650, 935)), //bottom right
				new Vector2(_random.Next(910, 1220), _random.Next(330, 420)),//rightmost space
			};

			Vector2 enemyPosition = areas[_random.Next(areas.Count)];	
			
			if(_enemies <= _maxEnemies && Globals.Enemies < 20)
			{
				Globals.Enemies++;
				_enemies++;
				GetNode<Timer>("SpawnTimer").WaitTime = 0.7f;
				KinematicBody2D enemy = (KinematicBody2D)Enemies[_random.Next(Enemies.Count)].Instance();
				enemy.Position = enemyPosition;
				GetParent().AddChild(enemy); 
				enemyPosition.x += 15;
				enemyPosition.y += 15;
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
