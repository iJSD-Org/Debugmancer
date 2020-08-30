using Godot;
using System;

public class Bullet : RigidBody2D
{	
	public void _on_Bullet_body_entered(RigidBody2D body)
	{
		if(!body.IsInGroup("player")) QueueFree();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
}






