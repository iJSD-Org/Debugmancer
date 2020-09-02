using Godot;
using System;
using Debugmancer.Objects.Player;

public class Scent : Node2D
{
	public Entity Entity;

    public override void _Ready()
    {
	    GetNode<Timer>("Timer").Connect("timeout", this, nameof(RemoveScent));
    }

    private void RemoveScent()
    {
	    Entity.ScentTrail.Remove(this);
		QueueFree();
    }
}
