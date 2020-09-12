using System;
using System.Collections.Generic;
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
				new Vector2(_random.Next(-425, -10), _random.Next(425, 600)),
				new Vector2(_random.Next(30, 105), _random.Next(100, 270)),
				new Vector2(_random.Next(30, 1210), _random.Next(335, 580)),
				new Vector2(_random.Next(30, 420), _random.Next(650, 935)),
				new Vector2(_random.Next(1050, 1380), _random.Next(635, 800)),
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
			Globals.scoreMultiplier += .5;
			GetNode<Timer>("SpawnTimer").WaitTime -= 0.2f;
		}
	}
}
