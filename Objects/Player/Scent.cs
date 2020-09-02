using Godot;
using System;
using Debugmancer.Objects.Player;

public class Scent : Node2D
{
	public Player Player;

    public override void _Ready()
    {
	    GetNode<Timer>("Timer").Connect("timeout", this, nameof(RemoveScent));
    }

    private void RemoveScent()
    {
	    Player.ScentTrail.Remove(this);
		QueueFree();
    }
}
