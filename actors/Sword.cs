using Godot;
using System;

public partial class Sword : Area2D
{
    private float damage = 10;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Monitoring = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void OnBodyEntered(Node body)
    {
        foreach (Node child in body.GetChildren())
        {
            if (child is Damageable damageable)
            {
                damageable.Hit(damage);
            }
            // Perform actions on each child here.
            GD.Print("Child name: " + child.Name);
        }
    }

}
