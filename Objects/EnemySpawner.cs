using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner : Node
{
    [Export] public PackedScene _roach = ResourceLoader.Load("res://Objects/Roach/Roach.tscn") as PackedScene;
    [Export] public PackedScene _enemy1 = ResourceLoader.Load("res://Objects/TempEnemy1/TempEnemy1.tscn") as PackedScene;
    [Export] public PackedScene _enemy2 = ResourceLoader.Load("res://Objects/TempEnemy2/TempEnemy2.tscn") as PackedScene;
    [Export] public PackedScene _enemy3 = ResourceLoader.Load("res://Objects/TempEnemy3/TempEnemy3.tscn") as PackedScene;
    [Export] public PackedScene _enemy4 = ResourceLoader.Load("res://Objects/TempEnemy4/TempEnemy4.tscn") as PackedScene;
    [Export] public PackedScene _enemy5 = ResourceLoader.Load("res://Objects/TempEnemy5/TempEnemy5.tscn") as PackedScene;
    private readonly Timer _spawnCD = new Timer();
    private float waitTime = 60f;
    private double enemyMultiplier = 1;

    public override void _Ready()
    {
        _spawnCD.Connect("timeout", this, "_on_spawnCD_timeout");
        _spawnCD.WaitTime = waitTime;
        AddChild(_spawnCD);
        _spawnCD.Start();
    }

    private void _on_spawnCD_timeout()
    {
        Random random = new Random();
        List<PackedScene> enemyList = new List<PackedScene> {_enemy1, _enemy2, _enemy3, _enemy4, _enemy5, _roach}; 
        List<Vector2> areas = new List<Vector2> {
            new Vector2(random.Next(-425, -10), random.Next(425, 600)), 
            new Vector2(random.Next(30, 105), random.Next(100, 270)), 
            new Vector2(random.Next(30, 1210), random.Next(335, 580)),
            new Vector2(random.Next(30, 420), random.Next(650, 935)),
            new Vector2(random.Next(1050, 1380), random.Next(635, 800)),
        };
        var enemyPosition = areas[random.Next(areas.Count)];
        for(double enemyAmount = Math.Ceiling(3 * enemyMultiplier); enemyAmount > 0; enemyAmount--)
        {
            KinematicBody2D enemy = (KinematicBody2D)enemyList[random.Next(enemyList.Count)].Instance();
            enemy.Position = enemyPosition;
            GetParent().AddChild(enemy);
            enemyPosition.x += 15;
            enemyPosition.y += 15;
        }
        enemyMultiplier += .1;
        waitTime -= 0.2f;
    }
}