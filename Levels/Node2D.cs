using Godot;
using System;

public class Node2D : Godot.Node2D
{
    public override void _Ready()
    {
        GD.Print("Hello, World!");
    }
}
