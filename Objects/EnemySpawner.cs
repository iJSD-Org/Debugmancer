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
		
		public void SpawnTimer_timeout()
		{
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
			for (double enemyAmount = Math.Ceiling(3 * EnemyMultiplier); enemyAmount > 0; enemyAmount--)
			{
				KinematicBody2D enemy = (KinematicBody2D)Enemies[_random.Next(Enemies.Count)].Instance();
				enemy.Position = enemyPosition;
				GetParent().AddChild(enemy); 
				enemyPosition.x += 15;
				enemyPosition.y += 15;
				GD.Print("EnemySpawned");
			}
			EnemyMultiplier += .3;
			Globals.ScoreMultiplier += .3;
			GetNode<Timer>("SpawnTimer").WaitTime -= 0.2f;
		}
	}
}
